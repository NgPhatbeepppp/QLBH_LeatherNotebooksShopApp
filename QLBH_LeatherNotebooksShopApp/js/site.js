function showToast(message, isError = false) {
    const toast = $("#toast");
    toast.text(message);
    toast.removeClass("error");

    if (isError) {
        toast.addClass("error");
    }

    toast.addClass("show");

    setTimeout(function () {
        toast.removeClass("show");
    }, 3000);
}

function addToCart(productId) {
    console.log("Đang thêm sản phẩm với ID:", productId);
    $.ajax({
        url: '/ShoppingCart/AddToCart',
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                showToast("Sản phẩm đã được thêm vào giỏ hànggg!");
                $("#cart-item-count").text(response.itemCount);
            } else {
                showToast(response.message || "Không thể thêm sản phẩm vào giỏ hàng.", true);
            }
        },
        error: function () {
            showToast("Đã xảy ra lỗi khi thêm sản phẩm vào giỏ hàng.", true);
        }
    });
}
