//ingresa registros POST
let estado = 0; let metodo = ''; let url = ""; 

async function cargarProductos() {
    try {
        const response = await fetch('https://localhost:44358/api/Producto');
        const datos = await response.json();
        if (datos.success) {
            const tabla = datos.data.map(producto => `
                <tr class="text-center">
                    <td>${producto.numSerie}</td>
                    <td>${producto.nombre}</td>
                    <td>${producto.tipo}</td>
                    <td>${producto.estado}</td>
                    <td>$ ${producto.valor.toLocaleString('es-ES')}</td> 
                    <td>${producto.fechaAdq}</td>
                    <td>${producto.vidaUtil} Años</td>
                    <td>$ ${producto.valorResidual.toLocaleString('es-ES')}</td> 
                    <td nowrap>
                        <button class="btn btn-warning" onclick="editar('${producto.numSerie}')">Editar</button>
                        <button class="btn btn-danger" onclick="eliminar('${producto.numSerie}')">Eliminar</button>
                        <button class="btn btn-primary" onclick="depreciar('${producto.numSerie}')">Depre</button>
                    </td>
                </tr>
            `).join('');
            document.getElementById('cargar_producto').innerHTML = tabla;
        }
    } catch (error) {
        console.error('Error al cargar los productos:', error);
    }
}
function depreciar(id) {
    window.location.href = `depreciacion.html?id=${id}`, '_blank';
}
function cargarEstado(){
    fetch('https://localhost:44358/api/EstadoProducto')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Estado</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.nombre}">${x.nombre} </option>
                   ` })
                document.getElementById('EstadoFiltro').innerHTML = select
            }
        })
}
cargarEstado()
//Carga el select 
async function cargarTipos() {
    try {
        const response = await fetch('https://localhost:44358/api/TipoProducto');
        const datos = await response.json();
        if (datos.success) {
            const opciones = datos.data.map(tipo => `<option value="${tipo.id}">${tipo.nombre}</option>`).join('');
            document.getElementById('Tipo').innerHTML = `<option selected disabled value="">Seleccione Tipo Producto</option>${opciones}`;
        }
    } catch (error) {
        console.error('Error al cargar los tipos de productos:', error);
    }
}
cargarTipos();

//INGRESA PRODUCTOS ACCEDE AL POST Y PUT
document.getElementById('btnSave').addEventListener('click', () => {
    const numS = document.getElementById('Num_Serie').value;
    const nombre = document.getElementById('Nombre').value;
    const tipo = document.getElementById('Tipo').value;
    const Valor_Adq = document.getElementById('Valor_Adq').value; 
    const fechaAdq = document.getElementById('Fecha_Adq').value;
    const vidaUtil = document.getElementById('Vida_Util').value;
    const valorR = document.getElementById('Valor_Residual').value; 
    if (numS === '' || nombre === '' || tipo === '' || Valor_Adq === '' || fechaAdq === '' || vidaUtil === '' || valorR === '') {
        Swal.fire('Error', 'Debe completar todos los campos con *.', 'error');
        return;
    }
    let producto = {
        numSerie: document.getElementById('Num_Serie').value.trim(),
        nombre: document.getElementById('Nombre').value.trim(),
        idTipo: document.getElementById('Tipo').value.trim(),
        valor: document.getElementById('Valor_Adq').value.trim(),
        fechaAdq: document.getElementById('Fecha_Adq').value.trim(),
        vidaUtil: document.getElementById('Vida_Util').value.trim(),
        valorResidual: document.getElementById('Valor_Residual').value.trim()
    }
    if(estado == 0){
        metodo = 'Post'
        url = 'https://localhost:44358/api/Producto'
    }else{
        metodo = 'Put'
        url = `https://localhost:44358/api/Producto/${producto.numSerie}`
        producto.idEstado = document.getElementById('Estado').value.trim()
        estado = 0
    } 
    fetch(url, {
        method: metodo,
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(producto)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, '', 'success')
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById('cargar_modal'))
                modal.hide()
                cargarProductos()
                limpiar()
            }
            else {
                Swal.fire(result.message, 'ERROR', 'warning')
            }
        })
})

//CARGA VALORES DE LA TABLA EN EL FORMULARIO 
async function editar(numSerie) {
    estado = 1;
    document.getElementById('Num_Serie').disabled = true;
    try {
        const response = await fetch(`https://localhost:44358/api/Producto/${numSerie}`);
        const result = await response.json();
        if (result.success) {
            const { numSerie, nombre, idTipo, valor, fechaAdq, vidaUtil, valorResidual, idEstado } = result.data;
            document.getElementById('Num_Serie').value = numSerie;
            document.getElementById('Nombre').value = nombre;
            document.getElementById('Tipo').value = idTipo;
            document.getElementById('Valor_Adq').value = valor;
            document.getElementById('Fecha_Adq').value = fechaAdq;
            document.getElementById('Vida_Util').value = vidaUtil;
            document.getElementById('Valor_Residual').value = valorResidual;
            document.getElementById('Estado').value = idEstado;
            const modal = new bootstrap.Modal(document.getElementById('cargar_modal'));
            modal.show();
        }
    } catch (error) {
        console.error('Error al editar el producto:', error);
    }
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
            fetch(`https://localhost:44358/api/Producto/${id}`, {
                method: 'delete'
            }).then(response => response.json())
                .then(r => {
                    if (r.success) {
                        Swal.fire(
                            'Eliminado',
                            'Su registro ha sido eliminado con éxito.',
                            'success'
                        )
                        cargarProductos()
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
        document.getElementById('Num_Serie').disabled = false
        cargarTipos()
    })
}
function filtrarPorPK(filtroRut,filas,posicion) {

    for (let i = 0; i < filas.length; i++) {
        const fila = filas[i];
        const celdaRut = fila.getElementsByTagName("td")[posicion]; 

        if (celdaRut) {
            const rut = celdaRut.textContent.trim().toUpperCase();

            if (rut.includes(filtroRut)) {
                fila.style.display = ""; 
            } else {
                fila.style.display = "none"; 
            }
        }
    }
}
function filtro_rut(){
    const filtroRut = document.getElementById("EstadoFiltro").value.trim().toUpperCase();
    const tabla = document.getElementById("cargar_producto");
    const filas = tabla.getElementsByTagName("tr");
    filtrarPorPK(filtroRut,filas,3)
}
function restablecerTabla() {
    const filtroRut = document.getElementById("EstadoFiltro");
    filtroRut.value = ""; 
    const tabla = document.getElementById("cargar_producto");
    const filas = tabla.getElementsByTagName("tr");
    for (let i = 0; i < filas.length; i++) {
        filas[i].style.display = ""; 
    }
}

cargarProductos()
window.limpiar = limpiar;  window.editar = editar; window.eliminar = eliminar; window.depreciar=depreciar; window.filtro_rut = filtro_rut;
window.restablecerTabla = restablecerTabla;

