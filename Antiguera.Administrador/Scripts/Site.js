//$(document).ready(function () {
//    AlterarCorLayout();

//    AlterarCorBarra();

//    closeModal();

//    excluirItem();

//    $("#salvarConfiguracoes").on("click", function () {
//        var data = { IdCorHeader: $("#IdCorHeader").val(), IdCorBarra: $("#IdCorBarra").val() };

//        $.ajax({
//            url: "/Configuracao/Index",
//            data: data,
//            type: 'POST',
//            beforeSend: function () {
//                $(".loading").css("display", "block");
//            },
//            success: function (data) {
//                if (data.success) {
//                    $(".col-success").css("display", "block");
//                    $(".alert-success").show();
//                    $(".col-success .alert-success").html("<button type='button' class='close alert-close'><span aria-hidden='true'>&times;</span></button><p class='text-center'>" + data.mensagem + "</p>");
//                } else {
//                    $(".col-danger").css("display", "block");
//                    $(".alert-danger").show();
//                    $(".col-danger .alert-danger").html("<button type='button' class='close alert-close'><span aria-hidden='true'>&times;</span></button><p class='text-center'>" + data.mensagem + "</p>");
//                }
//                closeModal();
//                $(".loading").css("display", "none");
//            },
//            error: function () {
//                $(".col-danger").css("display", "block");
//                $(".alert-danger").show();
//                $(".col-danger .alert-success").html("<button type='button' class='close alert-close'><span aria-hidden='true'>&times;</span></button><p class='text-center'>Ocorreu um erro, tente novamente!</p>");
//                closeModal();
//                $(".loading").css("display", "none");
//            }
//        });
//    });
//});

//function AlterarCorLayout() {
//    $(".btn-layout-header").on("click", function () {
//        if ($(this).val() === "1") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-blue-light");
//            } else {
//                $("body").addClass("skin-blue");
//            }
//            AlterarImagemLogo();
//        } else if ($(this).val() === "2") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-yellow-light");
//            } else {
//                $("body").addClass("skin-yellow");
//            }
//            AlterarImagemLogo();
//        } else if ($(this).val() === "3") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-red-light");
//            } else {
//                $("body").addClass("skin-red");
//            }
//            AlterarImagemLogo();
//        } else if ($(this).val() === "4") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-purple-light");
//            } else {
//                $("body").addClass("skin-purple");
//            }
//            AlterarImagemLogo();
//        } else if ($(this).val() === "5") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-green-light");
//            } else {
//                $("body").addClass("skin-green");
//            }
//            AlterarImagemLogo();
//        } else if ($(this).val() === "6") {
//            removeSkin();
//            if ($("#IdCorBarra").val() === "1") {
//                $("body").addClass("skin-black-light");
//            } else {
//                $("body").addClass("skin-black");
//            }
//            AlterarImagemLogo();
//        }
//        $("#IdCorHeader").val($(this).val());
//    });
//}

//function AlterarCorBarra() {
//    $(".btn-layout-sidebar").on("click", function () {
//        $("#IdCorBarra").val($(this).val());
//        if ($(this).val() === "1") {
//            if ($("#IdCorHeader").val() === "1") {
//                removeSkin();
//                $("body").addClass("skin-blue-light");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "2") {
//                removeSkin();
//                $("body").addClass("skin-yellow-light");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "3") {
//                removeSkin();
//                $("body").addClass("skin-red-light");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "4") {
//                removeSkin();
//                $("body").addClass("skin-purple-light");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "5") {
//                removeSkin();
//                $("body").addClass("skin-green-light");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "6") {
//                removeSkin();
//                $("body").addClass("skin-black-light");
//                AlterarImagemLogo();
//            }
//        } else {
//            if ($("#IdCorHeader").val() === "1") {
//                removeSkin();
//                $("body").addClass("skin-blue");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "2") {
//                removeSkin();
//                $("body").addClass("skin-yellow");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "3") {
//                removeSkin();
//                $("body").addClass("skin-red");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "4") {
//                removeSkin();
//                $("body").addClass("skin-purple");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "5") {
//                removeSkin();
//                $("body").addClass("skin-green");
//                AlterarImagemLogo();
//            }
//            if ($("#IdCorHeader").val() === "6") {
//                removeSkin();
//                $("body").addClass("skin-black");
//                AlterarImagemLogo();
//            }
//        }
//    });
//}

//function removeSkin() {
//    if ($("body").hasClass("skin-blue")) {
//        $("body").removeClass("skin-blue");
//    } else if ($("body").hasClass("skin-blue-light")) {
//        $("body").removeClass("skin-blue-light");
//    } else if ($("body").hasClass("skin-green")) {
//        $("body").removeClass("skin-green");
//    } else if ($("body").hasClass("skin-green-light")) {
//        $("body").removeClass("skin-green-light");
//    } else if ($("body").hasClass("skin-red")) {
//        $("body").removeClass("skin-red");
//    } else if ($("body").hasClass("skin-red-light")) {
//        $("body").removeClass("skin-red-light");
//    } else if ($("body").hasClass("skin-yellow")) {
//        $("body").removeClass("skin-yellow");
//    } else if ($("body").hasClass("skin-yellow-light")) {
//        $("body").removeClass("skin-yellow-light");
//    } else if ($("body").hasClass("skin-black")) {
//        $("body").removeClass("skin-black");
//    } else if ($("body").hasClass("skin-black-light")) {
//        $("body").removeClass("skin-black-light");
//    } else if ($("body").hasClass("skin-purple")) {
//        $("body").removeClass("skin-purple");
//    } else if ($("body").hasClass("skin-purple-light")) {
//        $("body").removeClass("skin-purple-light");
//    }
//}

//function AlterarImagemLogo() {
//    if ($("body").hasClass("skin-black") || $("body").hasClass("skin-black-light")) {
//        $(".logo-mini img").attr("src", "/Content/Images/antiguera-mini-logo-2.fw.png");
//        $(".logo-lg img").attr("src", "/Content/Images/LogoAntiguera2.png");
//    } else {
//        $(".logo-mini img").attr("src", "/Content/Images/antiguera-mini-logo.fw.png");
//        $(".logo-lg img").attr("src", "/Content/Images/LogoAntiguera.png");
//    }
//}

//function closeModal() {
//    $(".alert-close").on("click", function () {
//        $(".alert").fadeOut();
//    });
//}

//function excluirItem() {
//    $(".btn-excluir").on("click", function () {
//        return confirm("Tem certeza que deseja excluir o item selecionado?");
//    });
//}