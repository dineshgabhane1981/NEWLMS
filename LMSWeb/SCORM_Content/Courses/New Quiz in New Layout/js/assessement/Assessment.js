
class Assessment {
    constructor(msg) {
        this._nav;
        this._nextBtn;
        this._prevBtn;
        this._submitBtn;
        this._retryBtn;
        this._swiper;
        this._data;
        this._loadCount;
        this._templates;
        this._reviewTemplates;
        this._scorm;
        this._timerInterval;
        this._initialTime;
        this._isReview;
        this._useranswers;
        this._htmlNode;
        this._score;
        this._reviewQCounter;
    }
    init() {
        this._nav = new Navigation();
        this._htmlNode = $(".quiz");
        this._isReview = false;
        this._loadCount = 0;
        this._score = 0;
        this._reviewQCounter = 0;
        this._templates = [];
        this._useranswers = [];
        window.onload = this.onWindowLoad;
        //window.onbeforeunload = this.onWindowClose;
        this._scorm = new LMSCommunication();
        
        this.loadData();
    };
    onWindowLoad(){
        //this._nav._swiper.update();
    }
    getLMSDataSuccess(){
        this.initHTML();
        this.decodeInteractionData(this._scorm.suspendData);
        this.dataLoadComplete(this._data);
    }
    getLMSDataFailed(){
        this.initHTML();
        this.dataLoadComplete(this._data);
        alert("SCORM 1.2 initialisation failed");
    }
    loadData() {
        $.getJSON("data/json/quizdata.json", function (data) {
            this._data = data;
            if(!this._scorm.connected){
                this._scorm.init(this._data.config.scormType, this.getLMSDataSuccess.bind(this), this.getLMSDataFailed.bind(this));
            }
        }.bind(this)).fail(function () {
            console.log("An error has occurred while loading quizdata.json file.");
        });
    }
    dataLoadComplete(data){
        this.loadTemplates(data.questions);
    }
    loadTemplates(questions){
        if(questions.length == 0){
            console.log("Quiz has 0 questions");
        }
        this.loadTemplate(questions);
    }
    loadTemplate(questions){
        var q = questions;

        if(this._loadCount === questions.length){
            this.loadTemplatesComplete();
            return;
        }
        
        var container = $('<div/>', {
            'class': 'swiper-slide swiper-side-'+this._loadCount
        });

        $(".swiper-wrapper").append(container);

        container.load("templates/"+q[this._loadCount].type+".html", function (html, status) {
            if(status === "success"){
                var inst = eval(`new ${this.capitalizeFirstLetter(q[this._loadCount].type)}()`);
                this._templates.push(inst);
                inst.init(this._loadCount, container, q[this._loadCount]);
                this._loadCount++;
                this.loadTemplate(q);
            }else{
                console.log("An error has occurred while loading "+"templates/"+q[this._loadCount].type+".html"+" file.");
            }
        }.bind(this))
    }
    loadTemplatesComplete(){
        this.updateContent(this._htmlNode, this._data.quiz);
        this._nav.init();
        this.quizTimer();
    }
    quizTimer(){
        var timerContainer = this._htmlNode.find(".quiz-time");
        if(this._data.quiz.duration != undefined || this._data.quiz.duration == 0){
            this.startTimer(timerContainer, this._data.quiz);
        }else{
            timerContainer.hide();
        }
    }
    initHTML() {
        this._nextBtn = this._htmlNode.find(".nav-button.next-button");
        this._prevBtn = this._htmlNode.find(".nav-button.prev-button");
        this._submitBtn = this._htmlNode.find(".nav-button.submit-button");
        this._retryBtn = this._htmlNode.find(".nav-button.retry-button");
        this._closeBtn = this._htmlNode.find(".nav-button.close-button");
        this._nextBtn.on("click", this.nextButtonClick.bind(this));
        this._prevBtn.on("click", this.prevButtonClick.bind(this));
        this._submitBtn.on("click", this.submitButtonClick.bind(this));
        this._retryBtn.on("click", this.retryButtonClick.bind(this));
        this._closeBtn.on("click", this.closeButtonClick.bind(this));
    }
    nextButtonClick(e) {
        if((this._nav.currentIndex+1) === this._nav.length-1){
            this._submitBtn.removeClass("hide");
            this._nextBtn.addClass("hide");
        }
        this._nav.next();
    }
    prevButtonClick(e) {
        if((this._nav.currentIndex-1) < this._nav.length-1){
            this._submitBtn.addClass("hide");
            this._nextBtn.removeClass("hide");
        }
        this._nav.prev();
    }
    submitButtonClick(e) {
        this._submitBtn.addClass("hide");
        this.reviewQuiz();
    }
    retryButtonClick(e){
        this._reviewQCounter = 0;
        this._htmlNode.find(".quiz-only").removeClass("hide");
        this._htmlNode.find(".review-only").addClass("hide");
        this._submitBtn.addClass("hide");
        this._nav.toSlide(0);
        this._templates.forEach(template => {
            template.reset();
        });
        while(this._reviewTemplates.length){
            var t = this._reviewTemplates.pop();
            t.removeEvents();
        }
        this._htmlNode.find(".review-quiz").empty();
        this.updateContent(this._htmlNode, this._data.quiz);
        this.quizTimer();
    }
    closeButtonClick(e){
        window.top.close();
    }
    capitalizeFirstLetter(string) {
        return string.charAt(0).toUpperCase() + string.slice(1);
    }
    updateContent(htmlNode, data){
        htmlNode.find(".quiz-title p").html(data.title);
        htmlNode.find(".name-label").html(data.nameLabel);
        htmlNode.find(".name-text").html(data.name);
        htmlNode.find(".desc-label").html(data.descLabel);
        htmlNode.find(".desc-text").html(data.description);
    }
    
    reviewQuiz(){
        //hide quiz only content
        this._htmlNode.find(".quiz-only").addClass("hide");
        //show review quiz content
        this._htmlNode.find(".review-only").removeClass("hide");
        if(this._data.quiz.multipleAttempts !== undefined && this._data.quiz.multipleAttempts){
            this._closeBtn.addClass("hide");
            this._retryBtn.removeClass("hide");
        }else{
            this._closeBtn.removeClass("hide");
            this._retryBtn.addClass("hide");
        }
        //remove timer from quiz
        this.removeTimer();
        this._reviewTemplates = [];
        this.updateContent(this._htmlNode, this._data.reviewQuiz);
        this._loadCount = 0;
        this.loadReviewTemplates(this._data.questions);
    }
    loadReviewTemplates(questions){
        if(questions.length == 0){
            console.log("Quiz has 0 questions");
        }
        this.loadReviewTemplate(questions);
    }
    loadReviewTemplate(questions){
        var q = questions;
        
        if(this._loadCount === questions.length){
            this.calculateScore();
            this.sendLMSData();
            return;
        }
        
        var type = q[this._loadCount].type;

        if(type == "video" || type == "audio"){
            this._loadCount++;
            this.loadReviewTemplate(q);
        }else{

            var container = $('<div/>', {
                'class': 'shadow review-question review-question-'+this._loadCount
            });
    
            this._htmlNode.find(".review-quiz").append(container);

            container.load("templates/"+type+".html", function (html, status) {
                if(status === "success"){
                    var inst = eval(`new ${this.capitalizeFirstLetter(q[this._loadCount].type)}()`);
                    this._reviewTemplates.push(inst);
                    inst.init(++this._reviewQCounter, container, q[this._loadCount], true, this._templates[this._loadCount].userAnswer);
                    this._loadCount++;
                    this.loadReviewTemplate(q);
                }else{
                    console.log("An error has occurred while loading "+"templates/"+type+".html"+" file.");
                }
            }.bind(this));
        } 
    }

    startTimer(htmlNode, data){
        var initialTime = new Date();
        this._timerInterval = setInterval(function(){
            var temp;
            var dt = new Date();

            var ms = dt.getTime() - initialTime.getTime();
            ms = Math.max(0, (data.duration * 60 * 1000 - ms));
            if(ms <= 0){
                var r = confirm(this._data.quiz.timeoutMessage);
                if(r || !r){
                    this.submitButtonClick();
                }
            }else{
                var hr = "000" + Math.floor(ms / (60 * 60 * 1000));
                var temp = (ms - (hr * 60 * 60 * 1000));
                var min = "0" + Math.floor(temp / (60 * 1000));
                var sec = temp - (min * 60 * 1000);
                temp = temp - (sec * 1000);
                sec = "0" + Math.floor(sec / 1000);
                var df = min.substr(min.length - 2) + ":" + sec.substr(sec.length - 2);
                var tt = data.timeLabel+df;
                htmlNode.find("p").html(tt);
            }
        }.bind(this), 1000);
    }
    removeTimer(){
        clearInterval(this._timerInterval);
    }
    close(){
        this.onWindowClose();
    }
    onWindowClose(){
        if(this._scorm && this._scorm.connected){
            console.log("close");
            this.removeTimer();
            this._scorm.closeCourse();
        }
    }
    calculateScore(){
        var passingScore = this._data.quiz.passingScore === undefined ? 80 : this._data.quiz.passingScore;
        var maxScore = this._data.quiz.maxScore === undefined || this._data.quiz.maxScore === 0 ? 100 : this._data.quiz.maxScore;
        var minScore = this._data.quiz.minScore === undefined ? 0 : this._data.quiz.minScore;
        var s = 0, t = 0;
        this._templates.forEach(element => {
            console.log(element.type)
            if(element.type === "mcq" || element.type === "mrq"){
                s += element.awardedPoints;
                t += element.totalPoints;
            }
        });
        var score = Math.floor(s/t*100);
        this._htmlNode.find(".score-container p").html(this._data.reviewQuiz.scoreLabel+""+score);
        
        if(this._scorm._version === this._scorm._SCORM24){
            if(score >= passingScore){
                this._scorm.lessonStatus = "completed"
                this._scorm.successStatus = "passed";
            }else if(score>0 && score < passingScore){
                this._scorm.lessonStatus = "incomplete"
                this._scorm.successStatus = "failed";
            }else{
                this._scorm.lessonStatus = "incomplete"
                this._scorm.successStatus = "failed";
            }
        }else{
            if(score >= passingScore){
                this._scorm.lessonStatus = "passed"
            }else if(score>0 && score < passingScore){
                this._scorm.lessonStatus = "failed"
            }else{
                this._scorm.lessonStatus = "failed"
            }
        }
        
        this._scorm.score = score.toString();
        var scaleDelta = (maxScore - minScore);
        scaleDelta = scaleDelta <= 0 ? 1 : scaleDelta;
        this._scorm.scaledScore = score/scaleDelta;
    }
    sendLMSData(){
        this.sendInteractionsToLMS();
        this.encodeInteractionData();
        this._scorm.setLMSData();
    }
    sendInteractionsToLMS(){
        this._templates.forEach(element => {
            if(element.type === "mcq" || element.type === "mrq" || element.type === "para"){
                console.log("sendInteractionsToLMS", element.isAnswered);
                if(element.isAnswered){
                    var questionId = "Question_"+element.id;
                    var questionText = element.data.questionText; 
                    var studentResponses = element.userAnswer;
                    var correctResponses = element.correctAnswer;
                    var wasCorrect = element.wasCorrect;
                    var questionType = (element.type === "mcq" || element.type === "mrq") ? "choice" : (element.type === "para") ? "fill-in" : "";
                    this._scorm.updateInteraction(questionId, questionText, studentResponses, correctResponses, wasCorrect, questionType)
                }
            }
        });
    }
    encodeInteractionData(){
        var temp = [];
        this._templates.forEach(element => {
            this._useranswers.push(element.userAnswer);
            temp.push(element.userAnswer.join("$"));
        });
        this._scorm.suspendData = temp.join("#");
        this._scorm.suspendData = this._scorm.totalInteractions + "$"+ this._scorm.suspendData;
    }
    decodeInteractionData(suspendData = ""){
        if(suspendData == "" || suspendData == undefined){
            return;
        }

        var temp = suspendData.split("$");
        this._scorm.totalInteractions = parseInt(temp[0]);
        temp[1].split("#").forEach(element => {
            this._useranswers.push(element.split("$"));
        });
    }
}
