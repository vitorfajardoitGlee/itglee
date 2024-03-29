﻿function isMobile() {
    const ua = navigator.userAgent;
    if (/(tablet|ipad|playbook|silk)|(android(?!.*mobi))/i.test(ua)) return true
    if (/Mobile|Android|iP(hone|od)|IEMobile|BlackBerry|Kindle|Silk-Accelerated|(hpw|web)OS|Opera M(obi|ini)/.test(ua)) return true;
    if (window.screen.availWidth < 1024) return true;
    return false;
}

function moveNavigation() {
    var navigation = document.querySelector('nav.cd-main-nav-wrapper');
    var isDeviceMobile = isMobile();
    if (!isDeviceMobile) {
        document.querySelector('header.header-menu').insertBefore(navigation, null);
    } else {
        document.getElementById("main").insertBefore(navigation, document.querySelector('div.cd-main-content').nextElementSibling);
    }
}

/*
 * element: Element that will be the target of scrolls
 * headerOffset: number of pixels that will offset window scroll
 */
function scrollSmoothToElement(element, headerOffset) {
    var headerOffset = 60;
    var elementPosition = element.getBoundingClientRect().top;
    var offsetPosition = elementPosition + window.pageYOffset - headerOffset;

    window.scrollTo({
        top: offsetPosition,
        behavior: "smooth"
    });
}

function initNav() {
    moveNavigation();

    window.addEventListener("resize", function () {
        (!window.requestAnimationFrame) ? setTimeout(moveNavigation, 300) : window.requestAnimationFrame(moveNavigation);
    });

    var myAnchors = document.querySelectorAll("a.menu-link:not(.link-navigation)");
    for (var i = 0; i < myAnchors.length; i++) {
        myAnchors[i].addEventListener("click",
            function (event) {
                event.preventDefault();
                try {
                    document.createDocumentFragment().querySelector(this.getAttribute("href"));
                }
                catch (e) {
                    window.location.href = this.getAttribute("href");
                }
                scrollSmoothToElement(document.querySelector(this.getAttribute("href")));
                myAnchors.forEach((elem) => { if (elem.classList.contains("active")) elem.classList.toggle("active"); })
                this.classList.toggle("active")
            },
            false);
    }

    Array.prototype.forEach.call(document.getElementsByClassName("cd-nav-trigger"), (elem) => {
        elem.addEventListener("click", function (e) {
            e.preventDefault();
            Array.prototype.forEach.call(document.querySelectorAll("header.header-menu, .cd-main-content, footer, .cd-main-nav"), function (elem, index, list) {
                elem.classList.toggle("nav-is-visible");
            });
        })
    });
}

function setupForm() {
    var form = document.getElementById("itglee-form");
    let params = new URLSearchParams(document.location.search);
    let jobid = params.get('JOB');
    form.setAttribute("data-value", jobid);
    var position = document.getElementById("form-position");
    var placeholder = document.getElementById("form-position-placeholder");
    position.value = jobid;
    placeholder.innerText = jobid;

    form.addEventListener("submit", function (event) {
        event.preventDefault();
        var errorcontainer = document.getElementById("form-error");
        errorcontainer.classList.add("hidden");
        errorcontainer.innerText = "";
        fetch(event.target.action, {
            method: form.method,
            body: new FormData(event.target),
            headers: {
                'Accept': 'multipart/form-data',
                'Access-Control-Allow-Headers': 'Accept',
                'Content-Type': 'multipart/form-data; charset=UTF-8'
            }
        }).then(response => {
            if (response.ok) {
                window.location.href = "/Careers"
            }
            else {
                let error = response.json();
                error.then(function (data) {
                    errorcontainer.innerText = data.value;
                });
                errorcontainer.classList.remove("hidden");
            }
        }).catch(error => {
            errorcontainer.innerHTML = "Oops! There was a problem submitting your form"
        })
    })
}

function initForm() {
    var form = document.getElementById("itglee-form");
    form.addEventListener("submit", function (event) {
        event.preventDefault();
    })
}