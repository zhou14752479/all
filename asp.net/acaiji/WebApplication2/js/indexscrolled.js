$(function () {
    $(window).scroll(function () {
        if ($(window).scrollTop() > 90) {
            if (!$(".navbar").hasClass("scrolled")) $(".navbar").addClass("scrolled");
            if (!$(".navbar").hasClass("scrolled-shadow")) $(".navbar").addClass("scrolled-shadow")
        } else {
            if ($(".navbar").hasClass("scrolled")) $(".navbar").removeClass("scrolled");
            if ($(".navbar").hasClass("scrolled-shadow")) $(".navbar").removeClass("scrolled-shadow")
        }
    });
})

