
class Navigation {
    constructor() {
        this._totalQuestions = 0;
        this._swiper;
    }
    get currentIndex(){
        return this._swiper.activeIndex;
    }
    get length(){
        return this._swiper.slides.length;
    }
    init() {
        this.initSwiper();
    };
    next() {
        this._swiper.slideNext();
    };
    prev() {
        this._swiper.slidePrev();
    };
    toSlide(num){
        this._swiper.slideTo(num);
    }
    initSwiper() {
        this._swiper = new Swiper('.swiper-container', {
            allowTouchMove:false,
            autoHeight:true,
            updateOnWindowResize:true,
            roundLengths:true,
            spaceBetween:20,
            pagination: {
                el: '.swiper-pagination',
                clickable: false,
                renderBullet: function (index, className) {
                    return '<span class="' + className + '">' + (index + 1) + '</span>';
                },
            },
        });
    }
    removeSwiper(){
        this._swiper.destroy();
    }
}
