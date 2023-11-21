const cargar_arriendos = () => {
    fetch('https://localhost:44358/api/Arriendo/Activo')
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
                        <input type="checkbox" id="checkbox-${x.id}" name="myCheckbox" data-duracion="${x.duracion}" data-valor="${x.valor}" data-id="${x.id}">
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

const cargar_cliente = () =>{
    fetch('https://localhost:44358/api/Cliente')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                console.log(datos.data)
                let select = '<option selected disabled value="">Seleccione Cliente</option>'
                datos.data.forEach(x => {
                    select += `
                        <option value="${x.rut}">${x.rut} (${x.nombre})</option>
                   ` })
                document.getElementById('filtroRut').innerHTML = select
            }
        })
}

function actualizarCalculos() {
    let valorBrutoTotal = 0;
    let checkboxesSeleccionados = document.querySelectorAll('input[name="myCheckbox"]:checked');

    checkboxesSeleccionados.forEach(checkbox => {
        const duracion = parseInt(checkbox.dataset.duracion);
        const valor = parseInt(checkbox.dataset.valor);
        valorBrutoTotal += duracion * valor;
    });

    document.getElementById('Bruto').value = valorBrutoTotal;

    // Calcular otros valores
    const descuentoInput = document.getElementById('Descuento').value;
    const descuento = descuentoInput ? parseFloat(descuentoInput) / 100 : 0;
    const valorNeto = Math.round((valorBrutoTotal * (1 - descuento)),1);
    document.getElementById('Neto').value = valorNeto;

    const total = Math.round(valorNeto * (1 + 0.19),1)
    document.getElementById('Total').value = total;
    document.getElementById('btnGuardar').disabled = false;
}

document.addEventListener('change', function(e) {
    if (e.target && e.target.name == 'myCheckbox') {
        actualizarCalculos();
    }
});

document.getElementById('Descuento').addEventListener('input', actualizarCalculos);

document.getElementById('btnBuscar').addEventListener('click', () => {
    const filtroRut = document.getElementById('filtroRut').value.trim().toUpperCase();
    const filas = document.getElementById('cargar_arriendos').getElementsByTagName('tr');
    filtrarPorPK(filtroRut, filas, 2); 
    
});

function filtrarPorPK(filtroRut, filas, posicion) {
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

function actualizarArriendos(idFactura) {
    function putPost(idArriendo,idFactura){
        fetch(`https://localhost:44358/api/Arriendo/${idArriendo}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {
                let arr = {
                    id: result.data.id,
                    idProducto: result.data.idProducto,
                    idCliente: result.data.idCliente,
                    inicio: result.data.inicio,
                    termino: result.data.termino,
                    valor: result.data.valor,
                    idFactura: idFactura,
                    idEstado: 2
                }
                fetch(`https://localhost:44358/api/Arriendo/${idArriendo}`, {
                    method: 'Put',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify(arr)
                }).then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            
                        }
                        else {
                            Swal.fire(result.message, 'ERROR', 'warning')
                        }
                    })
                
            }
        })
    }
    let checkboxesSeleccionados = document.querySelectorAll('input[name="myCheckbox"]:checked');
    checkboxesSeleccionados.forEach(checkbox => {
        
        let idArriendo = parseInt(checkbox.dataset.id, 10);
        putPost(idArriendo,idFactura)
    });
}

document.getElementById('btnGuardar').addEventListener('click', async () => {
    event.preventDefault();
    let factura = {
        bruto: document.getElementById('Bruto').value.trim(),
        descuento: document.getElementById('Descuento').value.trim(),
        neto: document.getElementById('Neto').value.trim(),
        iva: 19,
        total: document.getElementById('Total').value.trim(),
        fechaEmision: document.getElementById('Emision').value.trim(),
        fechaExpiracion: document.getElementById('Expiracion').value.trim(),
        idEstado: 1
    };

    if(factura.descuento == '') {
        factura.descuento = 0;
    }

    try {
        const response = await fetch('https://localhost:44358/api/Factura', {
            method: 'Post',
            headers: {
                'Content-type': 'application/json'
            },
            body: JSON.stringify(factura)
            
        });
        console.log(response)
        if (!response.ok) {
            throw new Error('Error en la solicitud: ' + response.statusText);
        }

        const result = await response.json();

        if (result.success) {
            Swal.fire(result.message, '', 'success');
            //pasar id de la factura creada
            let idFactura = result.data
            //llamar metodo actualizar arriendo
            actualizarArriendos(idFactura)
        } else {
            Swal.fire(result.message, 'ERROR', 'warning');
        }
    } catch (error) {
        console.error('Error al procesar la solicitud:', error);
        Swal.fire('Error al procesar la solicitud', error.toString(), 'error');
    }
});


cargar_cliente()
cargar_arriendos()
