function cargar_detalle(id){
    fetch(`https://localhost:44358/api/Factura/${id}`)
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                const x = datos.data; // Asumiendo que 'datos.data' es un objeto y no un arreglo
                let tabla = `
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
                    </tr>
                `;
                document.getElementById('cargar_detalle').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
}
function cargar_arriendos(id){
    fetch(`https://localhost:44358/api/Arriendo/Facturados/${id}`)
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
                    </tr>
                    `;
                });
                
                document.getElementById('cargar_arriendos').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
}

function obtenerIdDeURL() {
    const parametrosURL = new URLSearchParams(window.location.search);
    return parametrosURL.get('idFactura');
}


function iniciar() {
    const idFactura = obtenerIdDeURL();
    if(idFactura) {
        cargar_detalle(idFactura);
        cargar_arriendos(idFactura)
    } else {
        console.error('No se encontró el ID de la factura en la URL.');
    }
}


document.addEventListener('DOMContentLoaded', iniciar);


window.cargar_detalle = cargar_detalle; window.cargar_arriendos = cargar_arriendos;