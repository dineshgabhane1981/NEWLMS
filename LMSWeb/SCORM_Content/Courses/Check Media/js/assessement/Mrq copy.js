class Mrq {
    constructor() {
        this._id;
        this._data;
        this._correctAnswers;
        this._userAnswers;
        this._qType;
        this._htmlNode;
        this._optionCount;
        this._isReview;
    }
    init (id, htmlNode, data, isReview) {
        this._id = id;
        this._htmlNode = htmlNode;
        this._data = data;
        this._isReview = isReview;
        this._userAnswers = [];
        this._correctAnswers = data.answers;
        this.updateContent(htmlNode,data);
    };
    updateContent(htmlNode, data){
        $(htmlNode).find(".question-text p").html(data.questionText);
        $(htmlNode).find(".options-title p").html(data.instructionText);
        var options = data.options;
        options.forEach(e => {
            this._userAnswers.push(0);
            this.createOption(e)
        });  
    }
    createOption(data){
        this._htmlNode.find(".options").append($('<div/>', {
            'class': 'option option-'+this._optionCount
        }).append($('<label/>', {
            'class': 'option-container'
        }).append($('<span/>', {
            'class': 'checkbox-button'
        }).append($('<input/>', {
            'type': 'checkbox', 'id':'checkbox-'+this._optionCount
        })).append($('<span/>', {
            'class': 'checkmark'
        }))).append($('<span/>', {
            'class': 'option-text', 'html': data.text
        }))));
        this._optionCount++;
    }
    removeEvents(){
        
    }
    get userAnswer(){
        var self = this;
        var radioButtons = this._htmlNode.find('input');
        radioButtons.each(function(index) {
            if($(this).is(':checked')){
                self._userAnswers[index] = 1;
            }else{
                self._userAnswers[index] = 0;
            }
          });
        return self._userAnswers;
    }
    get correctAnswer(){
        return this._correctAnswers;
    }
}
