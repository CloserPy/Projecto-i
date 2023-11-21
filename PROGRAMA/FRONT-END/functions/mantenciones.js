let url = ""
//CARGA LA TABLA CON MANTENCIONES
const cargarMantenciones = async () => {
    try {
        const response = await fetch('https://localhost:44358/api/Mantenimiento');
        const datos = await response.json();
        if (datos.success) {
            const tabla = datos.data.map(x => `
                <tr>
                    <td>${x.id}</td>
                    <td>${x.producto} (${x.idProducto})</td>
                    <td>${x.tipo}</td>
                    <td>${x.inicio}</td>
                    <td>${x.termino}</td>
                    <td>${x.estado}</td>
                    <td>$ ${x.costo.toLocaleString('es-ES')}</td>
                    <td nowrap>
                        <button class="btn btn-warning" onclick="editar('${x.id}')">Editar</button>
                        <button class="btn btn-danger" onclick="eliminar('${x.id}','${x.idProducto}')">Eliminar</button>
                    </td>
                </tr>
            `).join('');

            document.getElementById('cargar_mantencion').innerHTML = tabla;
        }
    } catch (error) {
        console.error('Error al cargar los datos:', error);
    }
};
cargarMantenciones();


//CARGA EL SELECT DE PRODUCTOS
const cargarProducto= () => {
    fetch('https://localhost:44358/api/Producto')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Producto</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.numSerie}">${x.numSerie} (${x.nombre})</option>
                   ` })
                document.getElementById('NumSerie').innerHTML = select
            }
        })
}
cargarProducto()
const cargarTipo = () => {
    fetch('https://localhost:44358/api/TipoMantencion')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Tipo</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.id}">${x.nombre}</option>
                   ` })
                document.getElementById('Tipo').innerHTML = select
            }
        })
}
cargarTipo()
const cargarEstado= () => {
    fetch('https://localhost:44358/api/EstadoMantenimiento')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Estado</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.id}">${x.nombre}</option>
                   ` })
                document.getElementById('Estado').innerHTML = select
            }
        })
}
cargarEstado()
//CREAR Y EDITAR 
document.getElementById('btnSave').addEventListener('click', () => {
    url = 'https://localhost:44358/api/Mantenimiento'
    let metodo = document.getElementById('id').value == '' ? 'Post' : 'Put'
    let mantencion = {
        idProducto: document.getElementById('NumSerie').value.trim(),
        idTipo: document.getElementById('Tipo').value.trim(),
        inicio: document.getElementById('Fecha_In').value.trim(),
        termino: document.getElementById('Fecha_Te').value.trim(),
        idEstado: document.getElementById('Estado').value.trim(),
        costo: document.getElementById('Costo').value.trim()
    }
    if (document.getElementById('id').value != '') {  // Si trae un id se edita
        mantencion.id = document.getElementById('id').value.trim()
        url=`https://localhost:44358/api/Mantenimiento/${mantencion.id}` 
    }
    fetch(url, {
        method: metodo,
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(mantencion)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, '', 'success')
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById('cargar_modal'))
                modal.hide()
                //actualiza el producto a en mantencion si está activa la mantencion
                if(mantencion.idEstado == 2){
                    //carga los valores (get{id})
                    fetch(`https://localhost:44358/api/Producto/${mantencion.idProducto}`)
                    .then(respose => respose.json())
                    .then(result => {
                        if (result.success) {
                            let prod = {
                                numSerie:result.data.numSerie,
                                nombre:result.data.nombre,
                                idTipo:result.data.idTipo,
                                valor:result.data.valor,
                                fechaAdq:result.data.fechaAdq,
                                vidaUtil:result.data.vidaUtil,
                                valorResidual:result.data.valorResidual,
                                idEstado: 2
                            }
                            //actualiza el producto (put) anidado
                            fetch(`https://localhost:44358/api/Producto/${prod.numSerie}`, {
                                method: 'Put',
                                    headers: {
                                    'Content-type': 'application/json'
                                    },
                                body: JSON.stringify(prod)
                            }).then(response => response.json())
                                .then(result => {
                                if (result.success) {
                                   cargar_mantenciones()
                                }
                                else {
                                    Swal.fire(result.message, 'ERROR', 'warning')
                                }
                            })
                        }
                    })
                }else{
                    fetch(`https://localhost:44358/api/Producto/${mantencion.idProducto}`)
                    .then(respose => respose.json())
                    .then(result => {
                        if (result.success) {
                            let prod = {
                                numSerie:result.data.numSerie,
                                nombre:result.data.nombre,
                                idTipo:result.data.idTipo,
                                valor:result.data.valor,
                                fechaAdq:result.data.fechaAdq,
                                vidaUtil:result.data.vidaUtil,
                                valorResidual:result.data.valorResidual,
                                idEstado: 1
                            }
                            //actualiza el producto (put) anidado
                            fetch(`https://localhost:44358/api/Producto/${prod.numSerie}`, {
                                method: 'Put',
                                    headers: {
                                    'Content-type': 'application/json'
                                    },
                                body: JSON.stringify(prod)
                            }).then(response => response.json())
                                .then(result => {
                                if (result.success) {
                                    cargar_mantenciones()
                                }
                                else {
                                    Swal.fire(result.message, 'ERROR', 'warning')
                                }
                            })
                        }
                    })
                }
            }
            else {
                Swal.fire(result.message, 'ERROR', 'warning')
            }
        })
})
const editar = (id) => {
    document.getElementById('id').disabled = true
    fetch(`https://localhost:44358/api/Mantenimiento/${id}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {
                document.getElementById('id').value = result.data.id,
                document.getElementById('NumSerie').value = result.data.idProducto,
                document.getElementById('Tipo').value = result.data.idTipo,
                document.getElementById('Estado').value = result.data.idEstado,
                document.getElementById('Fecha_In').value = result.data.inicio,
                document.getElementById('Fecha_Te').value = result.data.termino,
                document.getElementById('Costo').value = result.data.costo
                const modal = new bootstrap.Modal(document.getElementById('cargar_modal'))
                modal.show()
            }
        })
}
async function mostrarDialogoConfirmacion() {
    return Swal.fire({
        title: `¿Está seguro de eliminar el registro?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Eliminar'
    });
}

// Función para obtener producto por número de serie
async function obtenerProducto(numeroSerie) {
    const response = await fetch(`https://localhost:44358/api/Producto/${numeroSerie}`);
    return response.json();
}

// Función para actualizar producto
async function actualizarProducto(producto) {
    const response = await fetch(`https://localhost:44358/api/Producto/${producto.numSerie}`, {
        method: 'Put',
        headers: { 'Content-type': 'application/json' },
        body: JSON.stringify(producto)
    });
    return response.json();
}

// Función para eliminar arriendo
async function eliminarMantencion(id) {
    const response = await fetch(`https://localhost:44358/api/Mantenimiento/${id}`, {
        method: 'delete'
    });
    return response.json();
}

// Función principal para eliminar registro
async function eliminar(id, numeroSerie) {
    try {
        const confirmacion = await mostrarDialogoConfirmacion();
        if (!confirmacion.isConfirmed) return;

        const respuestaProducto = await obtenerProducto(numeroSerie);
        if (!respuestaProducto.success) throw new Error(respuestaProducto.message);

        // Obtener solo los datos necesarios de la respuesta
        const { numSerie,nombre,idTipo,idEstado,fechaAdq,valor,vidaUtil,valorResidual } = respuestaProducto.data;

        // Construir el objeto prodActualizado con los datos necesarios
        let prodActualizado = { numSerie,nombre,idTipo,fechaAdq,valor,vidaUtil,valorResidual, idEstado: 1 }; // disponible

        const resultadoActualizacion = await actualizarProducto(prodActualizado);
        if (!resultadoActualizacion.success) throw new Error(resultadoActualizacion.message);

        // Procede a eliminar el arriendo
        const resultadoEliminacion = await eliminarMantencion(id);
        if (!resultadoEliminacion.success) throw new Error(resultadoEliminacion.message);

        Swal.fire('Eliminado', 'Su registro ha sido eliminado con éxito.', 'success');
        cargar_mantenciones()
    } catch (error) {
        Swal.fire(error.message, '', 'error');
    }
}
const limpiar = () => {
    document.querySelectorAll('.form-control').forEach(item => {
        item.value = ''
        document.getElementById('id').disabled = false
        cargarTipo()
        cargarProducto()
    })
}
cargarMantenciones()
window.limpiar = limpiar; window.cargar_mantenciones= cargarMantenciones;
window.editar = editar; window.eliminar = eliminar;

