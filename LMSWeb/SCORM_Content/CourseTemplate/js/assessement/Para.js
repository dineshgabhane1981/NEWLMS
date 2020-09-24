class Para {
    constructor() {
        this._id;
        this._data;
        this._qType;
        this._htmlNode;
        this._userAnswers;
        this._isReview;
        this._isCorrect;
    }
   init (id, htmlNode, data, isReview = false, userAnswers = []) {
        this._id = id;
        this._htmlNode = htmlNode;
        this._data = data; 
        this._qType = data.type;
        this._isReview = isReview;
        this._userAnswers = userAnswers;
        this._isCorrect = false;
        this.updateContent(htmlNode, data);
    };
    updateContent(htmlNode, data){
        $(htmlNode).find(".question-text p").html(data.questionText);
        var ta = $(htmlNode).find("textarea");
        ta.attr("placeholder", data.placeholderText);
        ta.attr("maxlength", 250);
        console.log(this._userAnswers.toString());
        console.log(this._isReview);
        if(!this._isReview){
            ta.bind('input propertychange', this.onTextChange.bind(this));
        }else{
            var v = this._userAnswers.toString().trim();
            ta.val(v);
            this._isAnswered = v.length !== 0;
            this._htmlNode.find(".question-counter p").html('Question '+this._id + (this.isAnswered ? "" : " - Not Attempted"));
            ta.attr("disabled", "disabled");
        }  
    }
    onTextChange(e){
        e.currentTarget.value = e.currentTarget.value.replace(/[#$<>]/g, '');
        this._userAnswers[0] = e.currentTarget.value;
    }
    removeEvents(){
        $(this._htmlNode).find("textarea").unbind('input propertychange', this.onTextChange.bind(this));
    }
    reset(){
        $(this._htmlNode).find("textarea").val("");
    }
    get userAnswer(){
        return this._userAnswers;
    }
    get correctAnswer(){
        return [1];
    }
    get wasCorrect(){
        return this._isCorrect;
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
        return this._isAnswered = this._userAnswers.toString().length > 0;
    }
}
