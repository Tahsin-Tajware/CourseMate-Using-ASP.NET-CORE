document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth'
            });
        }
    });
});
window.addEventListener('load', () => {
    document.body.classList.add('loaded');
});

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({ behavior: 'smooth' });
        }
    });
});

window.addEventListener('load', () => {
    document.body.classList.add('loaded');
    
    const groups = document.querySelectorAll('.menu-group[data-group]');
    groups.forEach(group => {
        const key = 'sidebar:' + group.dataset.group;
        const hasActiveChild = !!group.querySelector('a.active');
        const stored = localStorage.getItem(key);
        const shouldOpen = hasActiveChild || stored === '1';
        if (shouldOpen) {
            group.classList.add('open');
            const btn = group.querySelector('.menu-toggle');
            if (btn) btn.setAttribute('aria-expanded', 'true');
        }
    });
    
    document.querySelectorAll('.menu-toggle').forEach(btn => {
        btn.addEventListener('click', () => {
            const group = btn.closest('.menu-group');
            const open = group.classList.toggle('open');
            btn.setAttribute('aria-expanded', open ? 'true' : 'false');
            const key = 'sidebar:' + group.dataset.group;
            try {
                localStorage.setItem(key, open ? '1' : '0');
            } catch (e) {
                
            }
        });
    });
});
