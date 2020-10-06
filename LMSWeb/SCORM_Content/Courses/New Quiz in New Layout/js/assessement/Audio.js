class Audio {
    constructor() {
        this._id;
        this._data;
        this._qType;
        this._htmlNode;
        this._userAnswers;
        this._audioPlayer;
        this._isReview;
    }
    init (id, htmlNode, data, isReview = false) {
        this._id = id;
        this._htmlNode = htmlNode;
        this._data = data;
        this._qType = data.type;
        this._userAnswers = [];
        this._isReview = isReview;
        this.updateContent(htmlNode,data);
    };
    updateContent(htmlNode, data){
        $(htmlNode).find(".aud-title p").html(data.questionText);
        this._audioPlayer = $(htmlNode).find("audio");
        this._audioPlayer.attr("id", "aud-"+this._id)
        this._audioPlayer.attr("src", data.path);
        this._audioPlayer.on("ended", this.onPlayComplete.bind(this))
        this._audioPlayer[0].load();
        this._userAnswers[0] = 0;
    }
    removeEvents(){
        this._audioPlayer.off("ended");
        this._audioPlayer[0].pause();
        this._audioPlayer.attr('src',''); // empty source
        this._audioPlayer[0].load();
    }
    onPlayComplete(){
        this._userAnswers[0] = 1;
    }
    reset(){
        this._audioPlayer[0].load();
        this._audioPlayer[0].pause();
    }
    get userAnswer(){
        return this._userAnswers;
    }
    get correctAnswer(){
        return [1];
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
}
