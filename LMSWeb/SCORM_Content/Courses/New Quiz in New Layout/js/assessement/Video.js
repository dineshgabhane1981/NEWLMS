class Video {
    constructor() {
        this._id;
        this._data;
        this._qType;
        this._htmlNode;
        this._userAnswers;
        this._videoPlayer;
        this._isReview;
    }
    init (id, htmlNode, data, isReview) {
        this._id = id;
        this._htmlNode = htmlNode;
        this._data = data;
        this._qType = data.type;
        this._isReview = isReview;
        this._userAnswers = [];
        this.updateContent(htmlNode,data);
    };
    updateContent(htmlNode, data){
        $(htmlNode).find(".vid-title p").html(data.questionText);
        this._videoPlayer = $(htmlNode).find("video");
        this._videoPlayer.attr("id", "vid-"+this._id);
        this._videoPlayer.attr("src", data.path);
        this._videoPlayer.on("ended", this.onPlayComplete.bind(this));
        this._videoPlayer[0].load();
        this._userAnswers[0] = 0;
    }
    removeEvents(){
        this._videoPlayer.off("ended");
        this._videoPlayer[0].pause();
        this._videoPlayer.attr('src',''); // empty source
        this._videoPlayer[0].load();
    }
    onPlayComplete(){
        this._userAnswers[0] = 1;
    }
    reset(){
        this._videoPlayer[0].load();
        this._videoPlayer[0].pause();
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
