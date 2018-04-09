    $(document).ready(function() {
      function readURL(input) {
        if (input.files && input.files[0]) {
          var reader = new FileReader();
          reader.onload = function(e) {
            $('#target')
              .attr('src', e.target.result);
          };
          reader.readAsDataURL(input.files[0]);
        }
      }
      $(".imgInp").change(function() {
        readURL(this);
      });
    });
