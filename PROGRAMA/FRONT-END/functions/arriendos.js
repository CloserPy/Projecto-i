let url = ""; 
//CARGAR TABLA
const cargar_arriendos = () => {
    fetch('https://localhost:44358/api/Arriendo')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let tabla = '';
                datos.data.forEach(x => {                  
                    tabla += `
                    <tr class="text-center">
                        <td>${x.id}</td>
                        <td>${x.idProducto}</td>
                        <td>${x.idCliente}</td>
                        <td>${x.inicio}</td>
                        <td>${x.termino}</td>
                        <td>${x.duracion} Días</td>
                        <td>${x.valor}</td>
                        <td>${x.estado}</td>
                        <td nowrap>
                            <button class="btn btn-warning" onclick="editar('${x.id}')">Editar</button>
                            <button class="btn btn-danger" onclick="eliminar('${x.id}','${x.idProducto}')">Eliminar</button>
                        </td>
                    </tr>
                    `;
                });
                document.getElementById('cargar_arriendos').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
};
//CARGA SELECT PRODUCTO
const cargarProducto = () => {
    fetch('https://localhost:44358/api/Producto')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let select = '<option selected disabled value="">Seleccione Producto</option>'
                datos.data.forEach(x => {
                    // Asumiendo que 'x.estado' contiene el estado del producto
                    // y excluyendo productos en estado 'EN OBRA' o 'MANTENCION'
                    if (x.estado !== 'EN OBRA' && x.estado !== 'MANTENCION') {
                        select += `<option value="${x.numSerie}">${x.numSerie} (${x.nombre})</option>`;
                    }
                });
                document.getElementById('Num_Serie').innerHTML = select;
            }
        })
        .catch(error => {
            console.error('Error al cargar los productos:', error);
        });
};


const cargarCliente= () => {
    fetch('https://localhost:44358/api/Cliente')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Cliente</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.rut}">${x.nombre} (${x.rut})</option>
                   ` })
                document.getElementById('Cliente').innerHTML = select
            }
        })
}


document.getElementById('btnSave').addEventListener('click', () => {
    url = 'https://localhost:44358/api/Arriendo'
    let metodo = document.getElementById('id').value == '' ? 'Post' : 'Put'
    let arriendo = {
        idProducto: document.getElementById('Num_Serie').value.trim(),
        idCliente: document.getElementById('Cliente').value.trim(),
        inicio: document.getElementById('Inicio').value.trim(),
        termino: document.getElementById('Termino').value.trim(),
        valor: document.getElementById('Valor').value.trim(),
        idEstado: 1,
    }
    if (document.getElementById('id').value != '') {  // Si trae un id se edita
        arriendo.id = document.getElementById('id').value.trim()
        url=`https://localhost:44358/api/Arriendo/${arriendo.id}` 
    }
    fetch(url, {
        method: metodo,
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(arriendo)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, '', 'success')
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById('cargar_modal'))
                modal.hide()
                cargar_arriendos()
            }
            else {
                Swal.fire(result.message, 'ERROR', 'warning')
            }
        })

    limpiar()
})

// Función para mostrar el diálogo de confirmación
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



const editar = (id) => {
    document.getElementById('Num_Serie').disabled = true
    document.getElementById('Cliente').disabled = true
    fetch(`https://localhost:44358/api/Arriendo/${id}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {
                document.getElementById('id').value = result.data.id,
                document.getElementById('Num_Serie').value = result.data.idProducto,
                document.getElementById('Cliente').value = result.data.idCliente,
                document.getElementById('Inicio').value = result.data.inicio,
                document.getElementById('Termino').value = result.data.termino,
                document.getElementById('Valor').value = result.data.valor
                const modal = new bootstrap.Modal(document.getElementById('cargar_modal'))
                modal.show()
            }
        })
}

const eliminar = (id) =>{
    
    Swal.fire({
        title: `¿Está seguro de eliminar el registro?`,
        text: '',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.isConfirmed) {
            fetch(`https://localhost:44358/api/Arriendo/${id}`, {
                method: 'delete'
            }).then(response => response.json())
                .then(r => {
                    if (r.success) {
                        Swal.fire(
                            'Eliminado',
                            'Su registro ha sido eliminado con éxito.',
                            'success'
                        )
                        cargar_arriendos()
                    }
                    else {
                        Swal.fire(
                            r.message,
                            '',
                            'error'
                        )
                    }
                })
        }

    })
}
const limpiar = () => {
    document.querySelectorAll('.form-control').forEach(item => {
        item.value = ''
        cargarCliente()
        cargarProducto()
    })
    document.getElementById('Num_Serie').disabled = false
    document.getElementById('Cliente').disabled = false
}
window.editar = editar; window.limpiar = limpiar; window.eliminar = eliminar;
cargar_arriendos()
cargarProducto()
cargarCliente()