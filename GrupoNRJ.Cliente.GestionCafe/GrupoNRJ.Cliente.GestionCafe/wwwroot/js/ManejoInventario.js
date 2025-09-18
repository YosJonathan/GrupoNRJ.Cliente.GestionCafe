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
            var textoGranos = $("#granosCreacion option:selected").text();
            var tostado = parseInt($("#tostadoCreacion").val()) || 0;
            const token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: urlAgregarProducto,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Nombre: nombre,
                    Cantidad: cantidad,
                    IdGrano: granos,
                    ValorMinimo: valorminimo,
                    NivelTostado: tostado,
                    Grano: textoGranos,
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

    $(".modificarProducto").click(function () {
        var id = $(this).attr("attr-valor");
        const token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: urlObtenerInfoProducto,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdProducto: id
            }),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (data) {
                $('#nombreModificar').val(data.nombre);
                $('#valorminimoModificar').val(data.cantidad);
                $('#granosModificar').val(data.grano);
                $('#tostadoModificar').val(data.nivel);
                $('#btnModificar').attr('attr-modificar', data.idp);
                var modal = new bootstrap.Modal(document.getElementById("modalModificar"));
                modal.show();
                console.log(data);
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
    $("#btnModificar").click(function () {
        let campos = document.querySelectorAll('#formModificar input, #formModificar select');
        let todosLlenos = true;

        campos.forEach(function (campo) {
            if (campo.value.trim() === '') {
                alert('El campo vacío tiene id: ' + campo.id); // Muestra el id del campo vacío
                todosLlenos = false;
                campo.style.border = '1px solid red'; // resalta campos vacíos
            } else {
                campo.style.border = ''; // quita el borde si está lleno
            }
        });
        if (todosLlenos) {


            var id = $("#btnModificar").attr("attr-modificar");
            const token = $('input[name="__RequestVerificationToken"]').val();
            var nombre = $('#nombreModificar').val();
            var valorMinimo = $('#valorminimoModificar').val();
            var granos = $('#granosModificar').val();
            var tostado = $('#tostadoModificar').val();


            $.ajax({
                url: urlModificarProducto,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    IdProducto: id,
                    Nombre: nombre,
                    GranoId: granos,
                    ValorMinimo: valorMinimo,
                    NivelTostado: tostado,
                }),
                headers: {
                    'RequestVerificationToken': token
                },
                success: function (data) {
                    if (data.todoCorrecto) {
                        Swal.fire({
                            title: data.todoCorrecto ? "Registro Ingresado Exitosamente!" : "Opps!",
                            icon: data.todoCorrecto ? "success" : "error",
                            text: data.todoCorrecto ? "" : "Ocurrio un error intentalo nuevamente"
                        }).then(() => {
                            if (data.todoCorrecto) location.reload();
                        });
                    } else {
                        Swal.fire({
                            title: "Opps!",
                            icon: "error",
                            text: "Ocurrio un error intentalo nuevamente"
                        });
                    }
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
    $('.movimientosProducto').click(function () {
        var idMovimiento = $(this).attr('attr-valor');
        llenarTablaMovimientos(idMovimiento);
    });

    $('#btnGuardarMovimiento').click(function () {
        var movimiento = $(this).attr("attr-valor");
        var cantidad = $('#movimientoProducto').val();
        const token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: urlAgregarMovimientoProducto,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdProducto: movimiento,
                Cantidad:cantidad,
            }),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (data) {
                if (data.todoCorrecto) {
                    Swal.fire({
                        title: data.todoCorrecto ? "Registro Ingresado Exitosamente!" : "Opps!",
                        icon: data.todoCorrecto ? "success" : "error",
                        text: data.mensaje
                    }).then(() => {
                        llenarTablaMovimientos(movimiento);
                    });

                } else {
                    Swal.fire({
                        title: "Opps!",
                        icon: "error",
                        text: "Ocurrio un error intentalo nuevamente"
                    });
                }
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

    function llenarTablaMovimientos(idMovimiento) {
        const token = $('input[name="__RequestVerificationToken"]').val();
        $.ajax({
            url: urlObtenerMovProducto,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdProducto: idMovimiento,
            }),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (data) {
                if (data.todoCorrecto) {
                    $('#btnGuardarMovimiento').attr('attr-valor', idMovimiento);
                    var modal = new bootstrap.Modal(document.getElementById("modalMovimientos"));
                    modal.show();

                    // 🔑 destruir si ya existe un DataTable
                    if ($.fn.DataTable.isDataTable('#tablaMovimientos')) {
                        $('#tablaMovimientos').DataTable().destroy();
                    }

                    // limpiar tbody
                    let tbody = $("#tablaMovimientos tbody");
                    tbody.empty();

                    // llenar manualmente las filas
                    $.each(data.registros, function (i, item) {
                        var icono = "";
                        if (item.tipoMovimiento =="Ingreso") {
                            icono = "<i class='bi bi-arrow-up text-success'></i>";
                        } else if (item.tipoMovimiento =="Salida") {
                            icono = "<i class='bi bi-arrow-down text-danger'></i>";
                        }

                        let fila = `
                <tr>
                    <td>${item.tipoMovimiento}</td>
                    <td>${item.cantidad} ${icono}</td>
                    <td>${item.fechaMovimiento}</td>
                </tr>
            `;
                        tbody.append(fila);
                    });

                    // volver a inicializar el DataTable
                    $('#tablaMovimientos').DataTable({
                        language: {
                            url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json"
                        }
                    });

                } else {
                    Swal.fire({
                        title: "Opps!",
                        icon: "error",
                        text: "Ocurrió un error, inténtalo nuevamente"
                    });
                }
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
});