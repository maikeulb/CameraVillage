  $(document).ready(function() {
    var button;
    $("#cart_items").on("click", "#cart-item", removeFromCart);

    function removeFromCart(e) {
      button = $(e.target);
      var productId = button.attr("data-catalog-item-id");
      alert(productId)
      removeFromCartCall(productId);
    };

    function removeFromCartCall(productId) {
      var contentTypeAttribute = 'application/json';
      var dataAttribute = JSON.stringify({
        productId: productId
      });
      $.ajax({
        url: "/api/cart/",
        method: "DELETE",
        contentType: contentTypeAttribute,
        data: dataAttribute,
        success: function(data) {
          updateCartCall();
          alert('cool')
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
          alert('whats good')
        },
        error: function(xhr, ajaxOptions, thrownError) {
          alert('something went wrong')
        },
      });
    }
  });
