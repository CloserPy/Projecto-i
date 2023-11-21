//CARGAR TABLA CON DEVOLUCIONES
let url = ''
const cargar_devoluciones = () => {
    fetch('https://localhost:44358/api/Devolucion')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let tabla = ''
                datos.data.forEach(x => {
                    tabla += `
                    <tr>
                    <td>${x.producto}</td>
                    <td>${x.cliente}</td>
                    <td>${x.fecha}</td>

                </tr>
                   ` })
                document.getElementById('cargar_devolucion').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
};
const cargarArriendo= () => {
    fetch('https://localhost:44358/api/Arriendo/Arrendados')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let select = '<option selected disabled value="">Seleccione Producto</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.id}">${x.idProducto} (${x.producto})</option>
                   ` })
                document.getElementById('Arriendo').innerHTML = select
            }
        })
}
cargarArriendo()
//REGISTRAR DEVOLUCION (actual)
document.getElementById('btnSave').addEventListener('click', () => {
    const fecha = document.getElementById('Fecha').value.trim();
    const arriendo = document.getElementById('Arriendo').value.trim();

    if(fecha === '' || arriendo === ''){
        Swal.fire('ERROR','Debe ingresar los campos obligatorios (*)','error')
        return;
    }
    url = 'https://localhost:44358/api/Devolucion'
    let metodo = document.getElementById('id').value == '' ? 'Post' : 'Put'
    let devolucion = {
        fecha: document.getElementById('Fecha').value.trim(),
        idArriendo: document.getElementById('Arriendo').value.trim()
    }
    fetch(url, {
        method: metodo,
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(devolucion)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, '', 'success')
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById('cargar_modal'))
                modal.hide()
                cargar_devoluciones()
            }
            else {
                Swal.fire(result.message, 'ERROR', 'warning')
            }
        })
})

cargar_devoluciones()