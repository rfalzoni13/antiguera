antiguera.core.configuracoes.imageUtility = antiguera.core.configuracoes.imageUtility || {
    clicarBotaoFoto: function (element, photoElement, type) {
        if (element.val().length > 0) {
            cancelarCorte(element, photoElement, type);
        }

        element.click();
    },

    cancelarCorte: function (element, photoElement, type) {
        element.val("");

        if(type == 'usuario')
            photoElement.attr("src", "/Content/Images/Profile/user.png");
    },

    mudarFoto: function (e, input, element, modalElement) {
        var target = e.target || e.srcElement;
        if (target.value.length === 0) {
            return null;
        }
        modalElement.modal({
            backdrop: 'static',
            keyboard: false
        });
        antiguera.core.configuracoes.imageUtility.carregarFoto(input, element);
    },

    carregarFoto: function (input, output) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                output.attr('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
            //return input.files[0].name;
        }
    },

    ajustarImagem: function (element) {
        var minCroppedWidth = 320;
        var minCroppedHeight = 320;
        var maxCroppedWidth = 2048;
        var maxCroppedHeight = 2048;
        element.cropper({
            autoCropArea: 0.5,
            viewMode: 2,
            responsive: true,
            data: {
                width: (minCroppedWidth + maxCroppedWidth) / 2,
                height: (minCroppedHeight + maxCroppedHeight) / 2
            },
            crop: function (event) {

                var width = event.detail.width;
                var height = event.detail.height;

                if (
                    width < minCroppedWidth
                    || height < minCroppedHeight
                    || width > maxCroppedWidth
                    || height > maxCroppedHeight
                ) {
                    antiguera.core.configuracoes.imageUtility.definirImagemCrop(Math.max(minCroppedWidth, Math.min(maxCroppedWidth, width)), Math.max(minCroppedHeight, Math.min(maxCroppedHeight, height)), element);
                }

                //console.log("Posição X é: " + event.detail.x);
                //console.log("Posição Y é: " + event.detail.y);
                //console.log("Largura é: " + event.detail.width);
                //console.log("Altura é: " + event.detail.height);
                //console.log("Rotação é: " + event.detail.rotate);
                //console.log("Escala X é: " + event.detail.scaleX);
                //console.log("Escala Y é: " + event.detail.scaleY);
            }
        });
    },

    definirImagemCrop: function (w, h, element) {
        var cropper = element.data('cropper');
        cropper.setData({
            width: w,
            height: h
        });
    },

    cortarImagem: function (element, photoElement, formData, name) {
        var cropper = element.data('cropper');
        var canvas = cropper.getCroppedCanvas();
        photoElement.attr("src", canvas.toDataURL());
        antiguera.core.configuracoes.imageUtility.prepararImagem(cropper, formData, name);
        $(".modal").modal("hide");
    },

    prepararImagem: function (cropper, formData, name) {
        cropper.getCroppedCanvas().toBlob((blob) => {

            // Pass the image file name as the third parameter if necessary.
            formData.append(name, blob);

        });
    },/*, 'image/png' */
}