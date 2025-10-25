$(document).ready(function () {
    $('#tablaPlanificacion').DataTable({
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.8/i18n/es-ES.json"
        },
        pageLength: 5
    });

    $(document).on('click', '#btnGuardar', function (e) {
        let campos = document.querySelectorAll('#formAgregar input, #formAgregar select');
        let todosLlenos = true;

        campos.forEach(function (campo) {
            if (campo.value.trim() === '') {
                console.log(campo.id);
                todosLlenos = false;
                campo.style.border = '1px solid red'; // resalta campos vacíos
            } else {
                campo.style.border = ''; // quita el borde si está lleno
            }
        });

        if (todosLlenos) {
            var inicio = $("#inicio").val();
            var fin = $("#fin").val();
            var estado = parseFloat($("#estado").val()) || 0;
            var lotes = parseInt($("#lotes").val()) || 0;
            const token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: urlCrearPlanificacion,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    IdPlani: 0,
                    IdLote: lotes,
                    IdEstado: estado,
                    FechaEstimada: inicio,
                    FechaFinEstimada: fin,
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
    $(".modificarProducto").click(function () {
        var id = $(this).attr("attr-valor");
        const token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            url: urlObtenerPlanificacion,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                IdPlanificacionLote: id
            }),
            headers: {
                'RequestVerificationToken': token
            },
            success: function (data) {
                asignarFechaInput(data.listado.fechaInicio, 'inicioM');
                asignarFechaInput(data.listado.fechaFin, 'finM');
                seleccionarOpcionPorTexto('estadoM', data.listado.estado);
                $('#lotesM').val(data.listado.idLote);
                ;
                $('#btnModificar').attr('attr-modificar', data.listado.idPlanificacion);
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
            var inicio = $("#inicioM").val();
            var fin = $("#finM").val();
            var estado = parseFloat($("#estadoM").val()) || 0;
            var lotes = parseInt($("#lotesM").val()) || 0;


            $.ajax({
                url: urlModificarPlanificacion,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    IdPlani: id,
                    IdLote: lotes,
                    IdEstado: estado,
                    FechaEstimada: inicio,
                    FechaFinEstimada: fin,
                }),
                headers: {
                    'RequestVerificationToken': token
                },
                success: function (data) {
                    if (data.todoCorrecto) {
                        Swal.fire({
                            title: data.todoCorrecto ? "Registro Modificado Exitosamente!" : "Opps!",
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
    $(document).on('click', '#btnGuardarL', function (e) {
        let campos = document.querySelectorAll('#formLote input, #formLote select');
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
            var cantidad = $("#cantidadL").val();
            var granos = $("#granosL").val();
            var tueste = $("#tuesteL").val();
            const token = $('input[name="__RequestVerificationToken"]').val();

            $.ajax({
                url: urlCrearLote,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({
                    Cantidad: cantidad,
                    TipoGrano: granos,
                    TipoTueste: tueste,
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
    function asignarFechaInput(fechaStr, inputId) {
        const input = document.getElementById(inputId);
        if (!input) {
            console.error(`No se encontró el input con id "${inputId}"`);
            return;
        }

        // Separar fecha y hora
        let [fecha, hora] = fechaStr.split(" ");
        if (!fecha) return;

        // Extraer día, mes y año
        let [dia, mes, anio] = fecha.split("/");

        // Si el input es tipo "datetime-local"
        if (input.type === "datetime-local" && hora) {
            // El input espera formato: YYYY-MM-DDTHH:mm
            let fechaISO = `${anio}-${mes}-${dia}T${hora.slice(0, 5)}`;
            input.value = fechaISO;
        }
        // Si el input es tipo "date"
        else if (input.type === "date") {
            // El input espera formato: YYYY-MM-DD
            let fechaISO = `${anio}-${mes}-${dia}`;
            input.value = fechaISO;
        }
        else {
            console.warn(`El input con id "${inputId}" no es de tipo "date" ni "datetime-local".`);
        }
    }
    function seleccionarOpcionPorTexto(selectId, textoDescripcion) {
        const select = document.getElementById(selectId);
        if (!select) {
            console.error(`No se encontró el select con id "${selectId}"`);
            return;
        }

        // Buscar la opción cuyo texto visible coincida (ignorando mayúsculas/minúsculas y espacios)
        const normalizado = textoDescripcion.trim().toLowerCase();
        let encontrada = false;

        for (let option of select.options) {
            if (option.text.trim().toLowerCase() === normalizado) {
                option.selected = true;
                encontrada = true;
                break;
            }
        }

        if (!encontrada) {
            console.warn(`No se encontró una opción con el texto "${textoDescripcion}" en el select "${selectId}".`);
        }
    }

});