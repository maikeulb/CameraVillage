  $(document).ready(function() {
    var button;

    $("#products").on("click", "#product-item", addToCart);

    function addToCart(e) {
      e.preventDefault();
      button = $(e.target);
      var productId = button.attr("data-product-id");
      addToCartCall(productId);
    };

    function addToCartCall(productId) {
      var contentTypeAttribute = 'application/json';
      var dataAttribute = JSON.stringify({
        productId: productId
      });
      $.ajax({
        url: "/api/cart/",
        method: "POST",
        contentType: contentTypeAttribute,
        data: dataAttribute,
        success: function(data) {
          updateCartCall();
        },
        error: function(xhr, ajaxOptions, thrownError) {
          alert('something went wrong')
        },
      });
    }

    function updateCartCall(productId) {
      var contentTypeAttribute = 'application/json';
      var dataAttribute = JSON.stringify({});
      $.ajax({
        url: "/api/cartComponent/",
        method: "POST",
        contentType: contentTypeAttribute,
        success: function(data) {
          new Noty({
            text: 'Item Added To Cart'
          }).setTimeout(2000).show();
          $('.cartstatus-badge').html(data.itemsCount);
        },
        error: function(xhr, ajaxOptions, thrownError) {
          alert('something went wrong')
        },
      });
    }

  });
