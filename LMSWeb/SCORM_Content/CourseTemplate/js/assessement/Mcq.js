class Mcq {
    constructor() {
        this._id;
        this._data;
        this._correctAnswers;
        this._userAnswers;
        this._qType;
        this._htmlNode;
        this._optionCount;
        this._isReview;
        this._isCorrect;
        this._isAnswered;
    }
    
   init (id, htmlNode, data, isReview = false, userAnswers = []) {
        this._id = id;
        this._htmlNode = htmlNode;
        this._data = data;
        this._isReview = isReview;
        this._optionCount = 0;
        this._isCorrect = false;
        this._isAnswered = false;
        this._userAnswers = userAnswers;
        this._options = [];
        this._correctAnswers = data.answer;
        this._qType = data.type;
        this.updateContent(htmlNode,data);
    };
    updateContent(htmlNode, data){
        //update question text
        $(htmlNode).find(".question-text p").html(data.questionText);
        //update instruction text
        $(htmlNode).find(".options-title p").html(data.instructionText);
        var options = data.options;
        options.forEach(e => {
            //create user answer array with default values if not review mode
            if(!this._isReview)
                this._userAnswers.push(0);
            //create option one by one
            this.createOption(e)
        });  
        //if review mode, show review content
        if(this._isReview){
            $(htmlNode).find(".review-only").removeClass("hide");
            $(htmlNode).find(".quiz-only").addClass("hide");
            this.createReviewContent();
        }else{
            $(htmlNode).find(".review-only").addClass("hide");
            $(htmlNode).find(".quiz-only").removeClass("hide");
            //Add events only if assessment mode
            this.addEvents();
        }
    }
    getIcon(index){
        //define option icon based on mode
        if(this._isReview){
            if(this._userAnswers[index] === 1){
                if(this._userAnswers[index] === this._correctAnswers[index]){
                    return "icon-tick"
                }else{
                    return "icon-cross"
                }
            }else{
                return "icon-radio-button"
            }
        }else{
            return "icon-radio-button"
        }
    }

    createOption(data){
        this._htmlNode.find(".options").append($('<div/>', {
            'class': 'option option-'+this._optionCount
        }).append($('<div/>', {
            'class': 'option-container'
        }).append(this._options[this._optionCount]=$('<a/>', {
            'class': 'radio-button '+ this.reviewClass(), 'id':'radio-button-'+this._optionCount, 'href':'javascript:void(0)'
        }).append($('<span/>', {
            'class': 'icon-container'
        }).append($('<span/>', {
            'class': 'icon '+this.getIcon(this._optionCount)
        }))).append($('<span/>', {
            'class': 'option-text-container'
        }).append($('<span/>', {
            'class': 'option-text', 'html': data.text
        }))))));
        this._optionCount++;
    }
    reviewClass(){
        var c = '';
        if(this._isReview === true){
            c += 'review';
            if(this._correctAnswers[this._optionCount] === 1){
                c+= ' correct';
            }
        } 
        return c;
    }
    createReviewContent(){
        //is questions answered
        
        this._htmlNode.find(".question-counter p").html('Question '+this._id + (this.isAnswered ? "" : " - Not Attempted"));
        //is question correctly answered
        this._isCorrect = this._userAnswers.toString() === this._correctAnswers.toString();
        this._htmlNode.find(".points p").html((this._isCorrect? 1:0)+'/'+this._data.points+' points');
        //for Mcq get option feedback based on user answer
        var cOpt;
        this._userAnswers.forEach(function(ele, index){
            if(ele === 1){
                cOpt = this._data.options[index].feedback
            }
        }.bind(this));
        //update feedback
        this._htmlNode.find(".feedback p").html(cOpt);
    }
    reset(){
        this._htmlNode.find(".radio-button").removeClass("selected hover");
        this._userAnswers.forEach(function(e,index){
            this._userAnswers[index] = 0;
        }.bind(this));
    }
    addEvents(){
        this._options.forEach(e => {
            e.on("mouseover", this.optionOver.bind(this));
            e.on("mouseout", this.optionOut.bind(this));
            e.on("click", this.optionClick.bind(this));
        });
    }
    removeEvents(){
        this._options.forEach(e => {
            e.off("mouseover", this.optionOver.bind(this));
            e.off("mouseout", this.optionOut.bind(this));
            e.off("click", this.optionClick.bind(this));
        });
    }
    optionOver(e){
        $(e.currentTarget).addClass("hover");
    }
    optionOut(e){
        $(e.currentTarget).removeClass("hover");
    }
    optionClick(e){
        this.reset();
        var id = parseInt($(e.currentTarget).attr("id").split("-")[2]);
        this._userAnswers[id] = 1;
        $(e.currentTarget).addClass("selected");
    }
    get userAnswer(){
        return this._userAnswers;
    }
    get correctAnswer(){
        return this._correctAnswers;
    }
    get wasCorrect(){
        return this._userAnswers.toString() === this._correctAnswers.toString();
    }
    get id(){
        return this._id;
    }
    get data(){
        return this._data;
    }
    get type(){
        return this._qType;
    }
    get totalPoints(){
        return this._data.points
    }
    get awardedPoints(){
        return (this.wasCorrect) ? this._data.points : 0;
    }
    get isAnswered(){
        return this._isAnswered = this._userAnswers.toString().indexOf("1") !== -1;
    }
}
