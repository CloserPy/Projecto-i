const cargar_facturas = () => {
    fetch('https://localhost:44358/api/Factura')
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                let tabla = ''
                datos.data.forEach(x => {
                    tabla += `
                <tr class="text-center">
                    <td>${x.id}</td>
                    <td>${x.bruto}</td>
                    <td>${x.descuento}%</td>
                    <td>${x.neto}</td>
                    <td>${x.iva}%</td>
                    <td>${x.total}</td>
                    <td>${x.fechaEmision}</td>
                    <td>${x.fechaExpiracion}</td>
                    <td>${x.estado}</td>
                    <td nowrap class="text-center">
                        <button class="btn btn-success" onclick="pagar(${x.id})">Pagar</button>
                        <button class="btn btn-danger" onclick="anular(${x.id})">Anular</button>
                        <button class="btn btn-primary" onclick="verDetalle(${x.id})">Detalle</button>
                    </td>
                </tr>
                   ` })
                document.getElementById('cargar_Factura').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
};

// Función para manejar el clic en el botón Detalle
function verDetalle(idFactura) {
    // Redirige a la página detalleFactura.html con el ID de la factura como parámetro en la URL
    window.location.href = `detalleFactura.html?idFactura=${idFactura}`, '_blank';
}
const limpiar = () => {
    document.querySelectorAll('.form-control').forEach(item => {
        item.value = ''
    })
}

const pagar = (id) => {
    fetch(`https://localhost:44358/api/Factura/${id}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {

                let factura = {
                    id: result.data.id,
                    bruto: result.data.bruto,
                    descuento: result.data.descuento,
                    neto: result.data.neto,
                    iva: result.data.iva,
                    total: result.data.total,
                    fechaEmision: result.data.fechaEmision,
                    fechaExpiracion: result.data.fechaExpiracion,
                    idEstado: 2
                }
                fetch(`https://localhost:44358/api/Factura/${id}`, {
                    method: 'Put',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify(factura)
                }).then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            Swal.fire(result.message, '', 'success')
                            cargar_facturas()
                            limpiar()
                        }
                        else {
                            Swal.fire(result.message, 'ERROR', 'warning')
                        }
                    })
            }
        })
}
const anular = (id) => {
    fetch(`https://localhost:44358/api/Factura/${id}`)
        .then(respose => respose.json())
        .then(result => {
            if (result.success) {

                let factura = {
                    id: result.data.id,
                    bruto: result.data.bruto,
                    descuento: result.data.descuento,
                    neto: result.data.neto,
                    iva: result.data.iva,
                    total: result.data.total,
                    fechaEmision: result.data.fechaEmision,
                    fechaExpiracion: result.data.fechaExpiracion,
                    idEstado: 1
                }
                fetch(`https://localhost:44358/api/Factura/${id}`, {
                    method: 'Put',
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify(factura)
                }).then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            Swal.fire(result.message, '', 'success')
                            cargar_facturas()
                            limpiar()
                        }
                        else {
                            Swal.fire(result.message, 'ERROR', 'warning')
                        }
                    })
            }
        })
}
cargar_facturas();
window.verDetalle = verDetalle; window.pagar = pagar; window.anular = anular;
