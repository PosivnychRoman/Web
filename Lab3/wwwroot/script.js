﻿
const slider = document.querySelector(".slider")

const trail = document.querySelector(".trail").querySelectorAll("div")

let value = 0

let trailValue = 0

let interval = 10000


const slide = (condition) => {

    clearInterval(start)

    condition === "increase" ? initiateINC() : initiateDEC()

    move(value, trailValue)

    animate()

    start = setInterval(() => slide("increase"), interval);
}

const initiateINC = () => {
    
    trail.forEach(cur => cur.classList.remove("active"));
    
    value === 50 ? value = 0 : value += 50;
    
    trailUpdate();
};

    
const initiateDEC = () => {
    
    trail.forEach(cur => cur.classList.remove("active"));
    
    value === 0 ? value = 50 : value -= 50;
    
    trailUpdate();
};



const move = (S, T) => {
    
    slider.style.transform = `translateX(-${S}%)`
    
    trail[T].classList.add("active")
}

const tl = gsap.timeline({ defaults: { duration: 0.6, ease: "power2.inOut" } })
tl.from(".bg", { x: "-100%", opacity: 0 })
    .from(".p", { opacity: 0 }, "-=0.3")
    .from(".h1", { opacity: 0, y: "30px" }, "-=0.3")
    .from(".button", { opacity: 0, y: "-40px" }, "-=0.5")


const animate = () => tl.restart()


const trailUpdate = () => {
    if (value === 0) {
        trailValue = 0
    } else if (value === 50) {
        trailValue = 1
    }
}


let start = setInterval(() => slide("increase"), interval)


document.querySelectorAll("svg").forEach(cur => {
    
    cur.addEventListener("click", () => cur.classList.contains("next") ? slide("increase") : slide("decrease"))
})


const clickCheck = (e) => {
   
    clearInterval(start)
    
    trail.forEach(cur => cur.classList.remove("active"))
    
    const check = e.target
    
    check.classList.add("active")

    
    if (check.classList.contains("box2")) {
        value = 0
    } else {
        value = 50
    }
    
    trailUpdate()

    move(value, trailValue)

    animate()

    start = setInterval(() => slide("increase"), interval)
}


trail.forEach(cur => cur.addEventListener("click", (ev) => clickCheck(ev)))


const touchSlide = (() => {
    let start, move, change, sliderWidth


    slider.addEventListener("touchstart", (e) => {

        start = e.touches[0].clientX
        sliderWidth = slider.clientWidth / trail.length
    })

    slider.addEventListener("touchmove", (e) => {

        e.preventDefault()

        move = e.touches[0].clientX

        change = start - move
    })

    const mobile = (e) => {

        change > (sliderWidth / 4) ? slide("increase") : null;

        (change * -1) > (sliderWidth / 4) ? slide("decrease") : null;

        [start, move, change, sliderWidth] = [0, 0, 0, 0]
    }

    slider.addEventListener("touchend", mobile)
})()