$(document).ready(function () {
    $('#tablaUsuarios').DataTable({
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json"
        }
    });
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
            const token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: urlAgregarProducto,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Nombre: nombre,
                    Cantidad: cantidad,
                    IdGrano: granos,
                    ValorMinimo: valorminimo
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
            url: urlEliminarProducto,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdProducto: id
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