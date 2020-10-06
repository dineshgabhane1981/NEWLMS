class LMSCommunication {
    constructor() {
        this._SCORMAPI;
        this._isConnected;
        this._version = 1.2;
        this._initalTime;
        this._bookmark = "";
        this._suspendData = "";
        this._scoreRaw = "";
        this._studentName = '';
        this._scaledScore = '';
        this._resume = '';
        this._totalInteractions;
        this._lessonStatus = "incomplete";
        this._successStatus = "unknown";

        this._BOOKMARK = "cmi.core.lesson_location";
        this._LESSONSTATUS = "cmi.core.lesson_status";
        this._SUCCESS_STATUS = "cmi.success_status";
        this._SUSPENDDATA = "cmi.suspend_data";
        this._SESSIONTIME = "cmi.core.session_time";
        this._SCORE = "cmi.core.score.raw";
        this._USER_RESPONCE = ".student_response";
        this._STUDENT_NAME = "cmi.core.student_name";
        this._SCALED_SCORE = "cmi.score.scaled";
        this._TIME = ".time";
        this._RESUME = "resume";

        this._SCORM12 = 1.2;
        this._SCORM24 = 2004;
    }
    init(scormVersion, successCallback, failCallback){
        this._initalTime = new Date();
        this._isConnected = false;
        this._version = scormVersion;
        this.totalInteractions = -1;
        this._SCORMAPI = pipwerks.SCORM;
        this._SCORMAPI.version = this._version.toString();
        this._SCORMAPI.handleExitMode = true;

        if(this._version == this._SCORM24){
            this._BOOKMARK = "cmi.location";
            this._LESSONSTATUS = "cmi.completion_status";
            this._SESSIONTIME = "cmi.session_time";
            this._USER_RESPONCE = ".learner_response";
            this._SCORE = "cmi.score.raw";
            this._STUDENT_NAME = "cmi.learner_name";
            this._TIME = ".timestamp";
        }

        this._isConnected = this._SCORMAPI.init();				
        
        if (this.connected) {
            this.getLMSData(successCallback);
        } else {
            failCallback();
        }
    }

    getLMSData(callBack){
        this.suspendData = this._SCORMAPI.get(this._SUSPENDDATA);
		this.bookmark = this._SCORMAPI.get(this._BOOKMARK);
		this.lessonStatus = this._SCORMAPI.get(this._LESSONSTATUS);
		this.score = this._SCORMAPI.get(this._SCORE);
		this.studentName = this._SCORMAPI.get(this._STUDENT_NAME);
		if (!this.studentName) {
			studentName = "Student Name";
        }
        if (this._version === this._SCORM24) {
			this._successStatus = this._SCORMAPI.get(this._SUCCESS_STATUS);
		}
        if(this.lessonStatus != "completed" || this.lessonStatus != "passed" || this.lessonStatus != "failed"){
            this.lessonStatus != "incomplete";
        }
        console.log("getLMSData - bookmark", this.bookmark)
        console.log("getLMSData - suspendData", this.suspendData)
        console.log("getLMSData - bookmark", this.lessonStatus)
        console.log("getLMSData - scoreRaw", this.score)
        console.log("getLMSData - studentName", this.studentName)
        console.log("getLMSData - lessonStatus", this.lessonStatus)
        callBack();
    }

    closeCourse() {
        if (this.connected) {
            var timeSpent = this.sessionTime();
            console.log("setLMSData - sessionTime", timeSpent)
            this._SCORMAPI.set(this._SESSIONTIME, timeSpent);
            this._SCORMAPI.quit();
        }
    }

    get sessionTime() {
        var dt = new Date();
        var time = this._initalTime;
        var n = dt.getTime() - time.getTime();
        

        if (this._version === this._SCORM24) {
            return this.convertMilliSecondsIntoSCORM24Time(n);
        } else {
            return this.millisecondsToCMIDuration(n);
        }
    }

    setLMSData() {
        console.log("SetLMSData - bookmark", this.bookmark)
        console.log("SetLMSData - suspendData", this.suspendData)
        console.log("SetLMSData - lessonStatus", this.lessonStatus)
        console.log("SetLMSData - scoreRaw", this.score)
        if (this._SCORMAPI !== null && this.connected) {
            this._SCORMAPI.set(this._BOOKMARK, this.bookmark);
            this._SCORMAPI.set(this._SUSPENDDATA, this.suspendData);
            this._SCORMAPI.set(this._LESSONSTATUS, this.lessonStatus);				
            if(this.isNumber(this.score)){
                this._SCORMAPI.set(this._SCORE, this.score);
            }	
            if (this._version === this._SCORM24 ) {
                this._SCORMAPI.set(this._SCALED_SCORE, this.scaledScore);
                this._SCORMAPI.set(this._SUCCESS_STATUS, this._successStatus);
            }		
            this._SCORMAPI.save();
        }
    }
     
    updateInteraction(questionId, questionText, studentResponses, correctResponses, wasCorrect, questionType) {
        var index = ++this.totalInteractions;
        var result = "correct";
        if (wasCorrect) {
            result = "correct";
        } else {
            result = "wrong";
            if (this._version === this._SCORM24) {
                result = "incorrect";
            }
        }       
        if (questionText.length >= 255) {
            questionText = questionText.slice(0, 250);
            questionText += "...";
        }
        this._SCORMAPI.set("cmi.interactions." + index + ".id", questionId);
        this._SCORMAPI.set("cmi.interactions." + index + ".type", questionType);
        this._SCORMAPI.set("cmi.interactions." + index + ".objectives.0.id", questionText);
        this._SCORMAPI.set("cmi.interactions." + index + ".correct_responses.0.pattern", correctResponses);
        this._SCORMAPI.set("cmi.interactions." + index + this._USER_RESPONCE, studentResponses);
        this._SCORMAPI.set("cmi.interactions." + index + ".result", result);
        this._SCORMAPI.set("cmi.interactions." + index + this._TIME, this.getInteractionsSessionTime(this._initalTime.getTime()));
        
        console.log("SetLMSData - Interaction", this.totalInteractions);

        //this._SCORMAPI.set("cmi.interactions." + index + ".latency", getInteractionsSessionTime(qStartTime));
    }
    getInteractionsSessionTime(qStartTime) {
        var dtm = new Date();       
        var n = dtm.getTime() - qStartTime;		
        if(this._version === this._SCORM12){
            return this.millisecondsToCMIDurationInteractions(n);
        }else{
            var t = new Date().toISOString().toString();
            return t.substring(0, t.lastIndexOf("."))
        }
        
    }
    isNumber(val){
		var isnum = /^\d+$/.test(val);
		return isnum;
    }
    /**
    SCORM 1.2 time format
    Convert duration from milliseconds to 0000:00:00.00 format
    **/
    millisecondsToCMIDuration(duration) {
        var hr = "000" + Math.floor(duration / (60 * 60 * 1000));
        var temp = (duration - (hr * 60 * 60 * 1000));
        var min = "0" + Math.floor(temp / (60 * 1000));
        var sec = temp - (min * 60 * 1000);
        temp = temp - (sec * 1000);
        var ms = "0" + Math.floor(temp / 1000);
        sec = "0" + Math.floor(sec / 1000);
        var df = hr.substr(hr.length - 4) + ":" + min.substr(min.length - 2) + ":" + sec.substr(sec.length - 2) + "." + ms.substr(ms.length - 2);
        return df;
    }
    /*SCORM 1.2 time format:Convert duration from milliseconds to 00:00:00 format*/
    millisecondsToCMIDurationInteractions(intDuration) {
        var hr = "0" + Math.floor(intDuration / (60 * 60 * 1000));
        var temp = (intDuration - (hr * 60 * 60 * 1000));
        var min = "0" + Math.floor(temp / (60 * 1000));
        var sec = temp - (min * 60 * 1000);
        temp = temp - (sec * 1000);
        var ms = "0" + Math.floor(temp / 1000);
        sec = "0" + Math.floor(sec / 1000);
        var lf = hr.substr(hr.length - 2) + ":" + min.substr(min.length - 2) + ":" + sec.substr(sec.length - 2);
        return lf;
    }
    /*SCORM 2004 time format (Ref. form ADL) */
    convertMilliSecondsIntoSCORM24Time(milliseconds) {
        var ScormTime = "";
        var HundredthsOfASecond;
        var Seconds;
        var Minutes;
        var Hours;
        var Days;
        var Months;
        var Years;
        var HUNDREDTHS_PER_SECOND = 100;
        var HUNDREDTHS_PER_MINUTE = HUNDREDTHS_PER_SECOND * 60;
        var HUNDREDTHS_PER_HOUR = HUNDREDTHS_PER_MINUTE * 60;
        var HUNDREDTHS_PER_DAY = HUNDREDTHS_PER_HOUR * 24;
        var HUNDREDTHS_PER_MONTH = HUNDREDTHS_PER_DAY * (((365 * 4) + 1) / 48);
        var HUNDREDTHS_PER_YEAR = HUNDREDTHS_PER_MONTH * 12;
        HundredthsOfASecond = Math.floor(milliseconds / 10);
        Years = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_YEAR);
        HundredthsOfASecond -= (Years * HUNDREDTHS_PER_YEAR);
        Months = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_MONTH);
        HundredthsOfASecond -= (Months * HUNDREDTHS_PER_MONTH);
        Days = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_DAY);
        HundredthsOfASecond -= (Days * HUNDREDTHS_PER_DAY);
        Hours = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_HOUR);
        HundredthsOfASecond -= (Hours * HUNDREDTHS_PER_HOUR);
        Minutes = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_MINUTE);
        HundredthsOfASecond -= (Minutes * HUNDREDTHS_PER_MINUTE);
        Seconds = Math.floor(HundredthsOfASecond / HUNDREDTHS_PER_SECOND);
        HundredthsOfASecond -= (Seconds * HUNDREDTHS_PER_SECOND);
        if (Years > 0) {
            ScormTime += Years + "Y";
        }
        if (Months > 0) {
            ScormTime += Months + "M";
        }
        if (Days > 0) {
            ScormTime += Days + "D";
        }
        if ((HundredthsOfASecond + Seconds + Minutes + Hours) > 0) {
            ScormTime += "T";
            if (Hours > 0) {
                ScormTime += Hours + "H";
            }
            if (Minutes > 0) {
                ScormTime += Minutes + "M";
            }
            if ((HundredthsOfASecond + Seconds) > 0) {
                ScormTime += Seconds;

                if (HundredthsOfASecond > 0) {
                    ScormTime += "." + HundredthsOfASecond;
                }
                ScormTime += "S";
            }
        }
        if (ScormTime === "") {
            ScormTime = "0S";
        }
        ScormTime = "P" + ScormTime;
        return ScormTime;
    }
    get connected(){
        return this._isConnected;
    }
    get bookmark() {
        return this._bookmark;
    }
    get suspendData() {
        return this._suspendData;
    }
    get lessonStatus() {
        return this._lessonStatus;
    }
    get successStatus(){
        return this._successStatus;
    }
    get score() {
        return this._scoreRaw;
    }
    get scaledScore(){
        return this._scaledScore;
    }
    get studentName() {
        return this._studentName;
    }
    get totalInteractions(){
        return this._totalInteractions;
    }

    set bookmark(value) {
        this._bookmark = value;
    }
    set suspendData(value) {
        this._suspendData = value;
    }
    set lessonStatus(value) {
        this._lessonStatus = value;
    }
    set successStatus(value){
        this._successStatus = value;
    }
    set score(value) {
        this._scoreRaw = value;
    }
    set scaledScore(value){
        this._scaledScore = value;
    }
    set studentName(value) {
        this._studentName = value;
    }
    set totalInteractions(value){
        this._totalInteractions = value;
    }
}
