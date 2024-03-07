 const marquee = document.getElementById("myMarquee");
    marquee.addEventListener("mouseenter", () => {
    marquee.stop();
    });
        marquee.addEventListener("mouseleave", () => {
        marquee.start();
    });

const active = document.querySelectorAll('.itemdanhmuc-li-a');
active.forEach(function(ac){
  ac.addEventListener('click',function(e){
    active.forEach(function(link){
      link.classList.remove("active");
    });
    ac.classList.add("active");
  })
})

let slideIndex = 0; // Bắt đầu từ slide đầu tiên
showSlides();

function plusSlides(n) {
  showSlides(slideIndex += n);
}

function currentSlide(n) {
  showSlides(slideIndex = n - 1);
}

function showSlides() {
  let i;
  let slides = document.getElementsByClassName("mySlides");
  let dots = document.getElementsByClassName("dot");

  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slideIndex++;
  if (slideIndex > slides.length) { slideIndex = 1 }

  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
  }

  slides[slideIndex - 1].style.display = "block";
  dots[slideIndex - 1].className += " active";

  setTimeout(showSlides, 2000); // Tự động chuyển slide sau mỗi 2 giây
}

// var index =1;
//  onchangeslidehinhanh = function(e) {
//     var imgs =["https://saodo.edu.vn/uploads/slider/21.jpg","https://saodo.edu.vn/uploads/slider/21.jpg","https://saodo.edu.vn/uploads/slider/36.jpg","https://saodo.edu.vn/uploads/slider/21.jpg"];
//     document.getElementById('img').src = imgs[index];
//     index++;
//     if(index == 3)
//     {
//        index= 0;
//     }
//   }
// setInterval(onchangeslidehinhanh,1000)

const slides = document.querySelector('.slides');
  let currentIndex = 0;

  function showSlide(index) {
    const slideWidth = document.querySelector('.slide').clientWidth;
    slides.style.transform = `translateX(${-index * slideWidth}px)`;

    const allSlides = document.querySelectorAll('.slide');
    allSlides.forEach((slide, i) => {
      slide.style.opacity = i === index ? 1 : 0; // Hiển thị ảnh hiện tại và ẩn các ảnh khác
    });
  }

  function nextSlide() {
    currentIndex = (currentIndex + 1) % document.querySelectorAll('.slide').length;
    showSlide(currentIndex);
  }

  function prevSlide() {
    currentIndex = (currentIndex - 1 + document.querySelectorAll('.slide').length) % document.querySelectorAll('.slide').length;
    showSlide(currentIndex);
  }

  setInterval(nextSlide, 3000);
