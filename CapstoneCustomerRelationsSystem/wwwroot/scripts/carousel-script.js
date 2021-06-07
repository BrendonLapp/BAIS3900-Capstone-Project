//element references
var slidesContainer;
var slides;
var rightButton;
var leftButton;
var dotsContainer;
var dots;

//values
var slideWidth;

//functions
var setSlidePosition;
var changeSlideTo;
var updateDots;
var hideShowArrows;

window.onload = function () {
    initializeValues();
    //Use for transitioning slides left and right
    // moveSlidesIntoPosition();
    addRequiredClasses();
    addEventListeners();
}//end onload

function initializeValues () {
    slidesContainer = document.querySelector('#CarouselSlidesContainer');
    slides = Array.from(slidesContainer.children);
    rightButton = document.querySelector('#CarouselRightButton');
    leftButton = document.querySelector('#CarouselLeftButton');
    dotsContainer = document.querySelector('#CarouselDotsContainer');
    dots = Array.from(dotsContainer.children);

    slideWidth = slides[0].getBoundingClientRect().width;
}//end initializeValues

function moveSlidesIntoPosition () {
    setSlidePosition = (_slide, _index) => {
        _slide.style.left = slideWidth * _index + 'px';
    }

    //foreach slide, move the slides into position.
    slides.forEach(setSlidePosition);
}//end moveSlidesIntoPosition

function addRequiredClasses () {
    slides[0].classList.add('current-slide');
    dots[0].children[0].classList.add('current-slide');
}//end addRequiredClasses

updateDots = function(_currentDot, _chosenDot) {
    _currentDot.classList.remove('current-slide');
    _chosenDot.classList.add('current-slide');
}//end updateDots

hideShowArrows = function (_slides, _leftButton, _rightButton, _chosenIndex) {
    if (_chosenIndex === 0) {
        _leftButton.classList.add('hide-me');
        _rightButton.classList.remove('hide-me');
    } else if (_chosenIndex === _slides.length -1) {
        _leftButton.classList.remove('hide-me');
        _rightButton.classList.add('hide-me');
    } else {
        _leftButton.classList.remove('hide-me');
        _rightButton.classList.remove('hide-me');
    }
}//end hideShowArrows

changeSlideTo = function (_slidesContainer, _currentSlide, _chosenSlide) {
    //uncomment for translation movement.
    // _slidesContainer.style.transform = 'translateX(-' + _chosenSlide.style.left + ')';
    _currentSlide.classList.remove('current-slide');
    _chosenSlide.classList.add('current-slide');
}//end changeSlideTo


function addEventListeners() {
    //OnClick, make the current slide to the slide on the right.
    rightButton.addEventListener('click', _event => {
        const currentSlide = slidesContainer.querySelector('.current-slide');
        const rightSlide = currentSlide.nextElementSibling;
        const currentDot = dotsContainer.querySelector('.current-slide');
        const rightDot = currentDot.parentElement.nextElementSibling.children[0];
        const chosenIndex = slides.findIndex(_slide => _slide === rightSlide);

        changeSlideTo(slidesContainer, currentSlide, rightSlide);
        updateDots(currentDot, rightDot);
        hideShowArrows(slides, leftButton, rightButton, chosenIndex);
    });
    //OnClick, make the current slide to the slide on the right.
    leftButton.addEventListener('click', _event => {
        const currentSlide = slidesContainer.querySelector('.current-slide');
        const leftSlide = currentSlide.previousElementSibling;
        const currentDot = dotsContainer.querySelector('.current-slide');
        const leftDot = currentDot.parentElement.previousElementSibling.children[0];
        const chosenIndex = slides.findIndex(_slide => _slide === leftSlide);

        changeSlideTo(slidesContainer, currentSlide, leftSlide);
        updateDots(currentDot, leftDot);
        hideShowArrows(slides, leftButton, rightButton, chosenIndex);
    });
    dotsContainer.addEventListener('click', _event => {
        //Which dot was selected. 
        const chosenDot = _event.target.closest('button');
        if (chosenDot) {
            const currentSlide = slidesContainer.querySelector('.current-slide');
            const currentDot = dotsContainer.querySelector('.current-slide');
            const chosenIndex = dots.findIndex(_dot => _dot.children[0] === chosenDot);
            const chosenSlide = slides[chosenIndex];

            changeSlideTo(slidesContainer, currentSlide, chosenSlide);
            updateDots(currentDot, chosenDot);
            hideShowArrows(slides, leftButton, rightButton, chosenIndex);
        }
    });
}//end addEventListeners



