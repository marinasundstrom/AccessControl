window.domHelpers = {
    scrollToBottom: function (smooth = false, delay = 0) {
        setTimeout(() => {
            window.scrollTo({ top: document.body.scrollHeight, behavior: smooth ? 'smooth' : 'auto' });
        }, delay);
    }
}