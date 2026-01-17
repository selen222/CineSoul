$(document).ready(function () {
    $(document).on("click", ".add-to-watchlist-btn", function (e) {
        e.preventDefault();

        var btn = $(this);
        var movieId = btn.data("id");

        btn.prop('disabled', true);

        $.ajax({
            url: '/Movies/AddToList',
            type: 'POST',
            data: { movieId: movieId },
            headers: {
                "X-CSRF-TOKEN": $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (response) {
                btn.removeClass("btn-outline-light").addClass("btn-success");
                btn.html('<i class="fas fa-check me-2"></i> Eklendi');

                setTimeout(function () {
                    btn.removeClass("btn-success").addClass("btn-outline-light");
                    btn.html('<i class="fas fa-plus me-2"></i> Listeme Ekle');
                    btn.prop('disabled', false);
                }, 2000);
            },
            error: function (xhr) {
                if (xhr.status === 401) {
                    alert("Lütfen önce giriş yapınız.");
                } else {
                    alert("Bir hata oluştu. Lütfen tekrar deneyiniz.");
                }
                btn.prop('disabled', false);
            }
        });
    });
});