$(document).ready(function () {

    $("#btnGuardar").click(function () {
        let campos = document.querySelectorAll('#formAgregar input, #formAgregar select');
        let todosLlenos = true;

        campos.forEach(function (campo) {
            if (campo.value.trim() === '') {
                todosLlenos = false;
                campo.style.border = '1px solid red'; // resalta campos vacíos
            } else {
                campo.style.border = ''; // quita el borde si está lleno
            }
        });

        if (todosLlenos) {
            var nombre = $("#nombre").val();
            var cantidad = parseFloat($("#cantidad").val()) || 0; // convierte a número
            var valorminimo = parseFloat($("#valorminimo").val()) || 0;
            var granos = parseInt($("#granosCreacion").val()) || 0;
            var textoGranos = $("#granosCreacion option:selected").text();
            var tipoProducto = parseInt($("#tipoProductoCreacion").val()) || 0;

            const token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: urlAgregarCombo,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    nombre: $('#nombre').val(),
                    cafe: {
                        codigo: parseInt($('#cafeCreacion').val()),
                        descripcion: $('#cafeCreacion option:selected').val()
                    },
                    tasa: {
                        codigo: parseInt($('#tasaCreacion').val()),
                        descripcion: $('#tasaCreacion option:selected').val()
                    },
                    filtro: {
                        codigo: parseInt($('#filtroCreacion').val()),
                        descripcion: $('#filtroCreacion option:selected').val()
                    }
                }),
                headers: {
                    'RequestVerificationToken': token
                },
                success: function (data) {
                    Swal.fire({
                        title: data.todoCorrecto ? "Registro Ingresado Exitosamente!" : "Opps!",
                        icon: data.todoCorrecto ? "success" : "error",
                        text: data.todoCorrecto ? "" : "Ocurrio un error intentalo nuevamente"
                    }).then(() => {
                        if (data.todoCorrecto) location.reload();
                    });
                },
                error: function (err) {
                    Swal.fire({
                        title: "Opps!",
                        icon: "error",
                        text: "Ocurrio un error intentalo nuevamente"
                    });
                }
            });
        }
        else {
            Swal.fire({
                title: "Debe llenar todos los campos!",
                icon: "warning"
            });
        }
    });

    $(".eliminarProducto").click(function () {
        var id = $(this).attr("attr-valor");
        const token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: urlEliminarCombo,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdCombo: id
            }),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (data) {
                Swal.fire({
                    title: data.todoCorrecto ? "Registro Eliminado Exitosamente!" : "Opps!",
                    icon: data.todoCorrecto ? "success" : "error",
                    text: data.todoCorrecto ? "" : "Ocurrio un error intentalo nuevamente"
                }).then(() => {
                    if (data.todoCorrecto) location.reload();
                });
            },
            error: function (err) {
                Swal.fire({
                    title: "Opps!",
                    icon: "error",
                    text: "Ocurrio un error intentalo nuevamente"
                });
            }
        });
    });

});