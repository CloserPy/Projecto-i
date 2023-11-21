//falta formatear rut
//LISTAR CLIENTES EN LA TABLA
const cargar_clientes = () => {
    fetch('https://localhost:44358/api/Cliente')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let tabla = ''
                datos.data.forEach(x => {
                    tabla += `
                    <tr>
                    <td>${x.nombre}</td>
                    <td>${x.rut}</td>
                    <td>${x.tipo}</td>
                    <td>${x.direccion}</td>
                    <td>${x.telefono}</td>
                    <td nowrap class="text-center">
                        <button class="btn btn-warning" onclick="editar('${x.rut}')">Editar</button>
                        <button class="btn btn-danger" onclick="eliminar('${x.rut}')">Eliminar</button>
                    </td>
                </tr>
                   ` })
                document.getElementById('cargar_cliente').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
};
//CARGA EL SELECT CON VALORES DE LA TABLA TIPO_CLIENTE
const cargarSelect = () => {
    fetch('https://localhost:44358/api/TipoCliente')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let select = '<option selected disabled value="">Seleccione Tipo Cliente</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.id}">${x.nombre}</option>
                   ` })
                document.getElementById('Tipo').innerHTML = select
            }
        })
}
cargarSelect()

// INGRESA DATOS ACCEDE AL METODO PUT Y POST
let estado = 0; let metodo = ''; let url = ""
document.getElementById('btnSave').addEventListener('click', () => {
    estado = 0
    //Validacion de campos vacios
    const rut = document.getElementById('Rut').value.trim();
    const nombre = document.getElementById('Nombre').value.trim();
    const tipo = document.getElementById('Tipo').value.trim();
    const telefono = document.getElementById('Telefono').value.trim();
    const direccion= document.getElementById('Direccion').value.trim();

    if (rut === '' || nombre === '' || tipo === '' || telefono === '' || direccion === '') {
        Swal.fire('Error', 'Debe completar todos los campos con *.', 'error');
        return;
    }
    let cliente = {
        rut: document.getElementById('Rut').value.trim(),
        nombre: document.getElementById('Nombre').value.trim(),
        idTipo: document.getElementById('Tipo').value.trim(),
        direccion: document.getElementById('Direccion').value.trim(),
        telefono: document.getElementById('Telefono').value.trim()
    }
    if(estado == 0){
        metodo = 'Post'
        url = 'https://localhost:44358/api/Cliente'
    }else{
        metodo = 'Put'
        url = `https://localhost:44358/api/Cliente/${cliente.rut}`
    }

    fetch(url, {
        method: metodo,
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(cliente)
    }).then(response => response.json())
        .then(result => {
            if (result.success) {
                Swal.fire(result.message, '', 'success')
                const modal = bootstrap.Modal.getInstance(
                    document.getElementById('cargar_modal'))
                modal.hide()
                cargar_clientes()
                limpiar()
            }
            else {
                Swal.fire(result.message, 'ERROR', 'warning')
            }
        })

})

//Carga los datos en el formulario para editarse
const editar = (Rut) => {
    estado = 1
    document.getElementById('Rut').disabled = true
    fetch(`https://localhost:44358/api/Cliente/${Rut}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {
                document.getElementById('Rut').value = result.data.rut
                document.getElementById('Nombre').value = result.data.nombre
                document.getElementById('Tipo').value = result.data.idTipo
                document.getElementById('Direccion').value = result.data.direccion
                document.getElementById('Telefono').value = result.data.telefono
                const modal = new bootstrap.Modal(document.getElementById('cargar_modal'))
                modal.show()
            }
        })
}

//ELIMINA LOS CLIENTES
const eliminar = (rut) =>{
    
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
            fetch(`https://localhost:44358/api/Cliente/${rut}`, {
                method: 'delete'
            }).then(response => response.json())
                .then(r => {
                    if (r.success) {
                        Swal.fire(
                            'Eliminado',
                            'Su registro ha sido eliminado con éxito.',
                            'success'
                        )
                        cargar_clientes()
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
function formatearRUT(rut) {
    rut = rut.replace(/[^0-9kK]/g, '');
    if (rut.length > 1) {
        const numero = rut.slice(0, -1);
        const dv = rut.slice(-1);
        if (numero.length > 9) {
            numero = numero.slice(0, 9);
        }
        return `${numero.slice(0, 2)}.${numero.slice(2, 5)}.${numero.slice(5)}-${dv}`;
    } else {
        return rut;
    }
}

document.getElementById('Rut').addEventListener('input', () => {
    const rut = document.getElementById('Rut').value.trim();
    const rutFormateado = formatearRUT(rut);
    document.getElementById('Rut').value = rutFormateado;
});

const limpiar = () => {
    document.querySelectorAll('.form-control').forEach(item => {
        item.value = ''
        document.getElementById('Rut').disabled = false
        cargarSelect()
    })
}
//DAR FORMATO AL RUT
document.getElementById('Rut').addEventListener('input', () => {
    const rut = document.getElementById('Rut').value.trim(); const rutFormateado = formatearRUT(rut);
    document.getElementById('Rut').value = rutFormateado;
});
cargar_clientes()
//define como metodos globales  a los modulos importados
window.editar = editar; window.limpiar = limpiar;  window.eliminar = eliminar; window.formatearRUT = formatearRUT;
