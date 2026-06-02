document.addEventListener("DOMContentLoaded", function () {
    const filterButtons = document.querySelectorAll(".filter-btn");
    const articles = document.querySelectorAll(".news-card");

    if (filterButtons.length > 0 && articles.length > 0) {
        filterButtons.forEach(button => {
            button.addEventListener("click", function () {
                filterButtons.forEach(btn => btn.classList.remove("active"));
                this.classList.add("active");

                const targetCategory = this.getAttribute("data-category");

                articles.forEach(article => {
                    const articleCategory = article.getAttribute("data-cat-id");

                    if (targetCategory === "all" || articleCategory === targetCategory) {
                        article.style.display = "flex";
                    } else {
                        article.style.display = "none";
                    }
                });
            });
        });
    }
});
