
const menuOpenButton = document.querySelector('.top .menu-button');
const menuPanel = document.querySelector('nav');


menuOpenButton.addEventListener('click', () => {
    menuPanel.classList.toggle('active'); 
    if (menuPanel.classList.contains('active')) {
        menuOpenButton.focus();
        menuPanel.style.minWidth = 250 + "px";
        menuPanel.style.width = 250 + "px";
    }
});

document.addEventListener('click', (event) => {
    if (!event.target.closest('.top .menu-button') && !event.target.closest('nav')) {

        menuPanel.style.minWidth = 0;
        menuPanel.style.width = 0;
        setTimeout(() => menuPanel.classList.remove('active'), 500);
        
    }
});

window.addEventListener('resize', function () {
    if (window.innerWidth > 1000) {
        menuPanel.classList.remove('active');
        menuPanel.style.minWidth = 0;
        menuPanel.style.width = 0;
    }
    else {
    }
});
