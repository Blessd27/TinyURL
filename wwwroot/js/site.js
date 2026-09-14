// Copy-to-clipboard behavior for the generated short URL.
document.addEventListener("DOMContentLoaded", function () {
    var copyBtn = document.getElementById("copyBtn");
    if (!copyBtn) {
        return;
    }

    copyBtn.addEventListener("click", function () {
        var url = copyBtn.getAttribute("data-url");
        var input = document.getElementById("shortUrlInput");
        var label = document.getElementById("copyBtnText");

        function showCopied() {
            var original = label.textContent;
            label.textContent = "Copied!";
            copyBtn.classList.add("btn-success");
            copyBtn.classList.remove("btn-outline-primary");
            setTimeout(function () {
                label.textContent = original;
                copyBtn.classList.remove("btn-success");
                copyBtn.classList.add("btn-outline-primary");
            }, 1500);
        }

        if (navigator.clipboard && window.isSecureContext) {
            navigator.clipboard.writeText(url).then(showCopied);
        } else {
            // Fallback for browsers without the async Clipboard API.
            input.select();
            input.setSelectionRange(0, 99999);
            document.execCommand("copy");
            showCopied();
        }
    });
});
