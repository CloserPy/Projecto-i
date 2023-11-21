function cargar_producto(id){
    fetch(`https://localhost:44358/api/Producto/${id}`)
        .then(response => response.json())
        .then(datos => {
            if (datos.success) {
                const producto = datos.data; // Asumiendo que 'datos.data' es un objeto y no un arreglo
                let tabla = `
                    <tr class="text-center">
                    <td>${producto.numSerie}</td>
                    <td>${producto.nombre}</td>
                    <td>${producto.tipo}</td>
                    <td>${producto.estado}</td>
                    <td>$ ${producto.valor.toLocaleString('es-ES')}</td> 
                    <td>${producto.fechaAdq}</td>
                    <td>${producto.vidaUtil} Años</td>
                    <td>$ ${producto.valorResidual.toLocaleString('es-ES')}</td> 
                    </tr>
                `;
                document.getElementById('cargar_producto').innerHTML = tabla;
            }
        })
        .catch(error => {
            console.error('Error al cargar los datos:', error);
        });
}

function generarGraficoDepreciacion(valor, valorResidual, vidaUtil, fechaInicial) {
    const añoInicial = new Date(fechaInicial).getFullYear(); // Extrae el año de la fecha inicial
    const depreciacionAnual = (valor - valorResidual) / vidaUtil;
    let valores = [valor]; // Inicia con el valor inicial del producto
    let años = [añoInicial]; // Comienza en el año extraído de la fecha

    for (let año = 1; año <= vidaUtil; año++) {
        valores.push(valor - depreciacionAnual * año);
        años.push(añoInicial + año); // Añade los años siguientes
    }

    const ctx = document.querySelector('canvas').getContext('2d');

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: años,
            datasets: [{
                label: 'Valor del Producto',
                data: valores,
                backgroundColor: 'rgba(0, 123, 255, 0.5)',
                borderColor: 'rgba(0, 123, 255, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: false
                }
            },
            plugins: {
                legend: {
                    labels: {
                        color: 'black'
                    }
                },
                tooltip: {
                    mode: 'index',
                    intersect: false
                }
            },
            backgroundColor: 'white',
            onResize: function(chart, size) {
                chart.canvas.parentNode.style.backgroundColor = 'white';
            }
        }
    });

    ctx.canvas.parentNode.style.backgroundColor = 'white';
}

// Ejemplo de uso con una fecha inicial:
// generarGraficoDepreciacion(10000, 1000, 10, "2023-10-22");


// Ejemplo de uso:
// generarGraficoDepreciacion(10000, 1000, 10, 2012);





function obtenerIdDeURL() {
    const parametrosURL = new URLSearchParams(window.location.search);
    return parametrosURL.get('id');
}

async function generarGrafico(id){
    try {
        const response = await fetch(`https://localhost:44358/api/Producto/${id}`);
        const result = await response.json();
        if (result.success) {
            const { numSerie, nombre, idTipo, valor, fechaAdq, vidaUtil, valorResidual, idEstado } = result.data; //captura valores del producto
            generarGraficoDepreciacion(valor,valorResidual,vidaUtil,fechaAdq)
        }
    } catch (error) {
        console.error('Error al editar el producto:', error);
    }
}
function iniciar() {
    const id = obtenerIdDeURL();
    if(id) {
        cargar_producto(id);
        //generar grafico
        generarGrafico(id)

    } else {
        console.error('No se encontró el ID de la factura en la URL.');
    }
}


document.addEventListener('DOMContentLoaded', iniciar)

window.obtenerIdDeURL= obtenerIdDeURL; window.iniciar = iniciar;