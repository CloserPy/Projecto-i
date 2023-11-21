
        window.onload = function() {
            var fechaEmision = new Date();
            var mesEmision = fechaEmision.getMonth() + 1;
            var diaEmision = fechaEmision.getDate();
            var añoEmision = fechaEmision.getFullYear();
    
            // Asegúrate de que el día y el mes tengan dos dígitos
            if (diaEmision < 10) diaEmision = '0' + diaEmision;
            if (mesEmision < 10) mesEmision = '0' + mesEmision;
    
            var fechaFormateadaEmision = añoEmision + '-' + mesEmision + '-' + diaEmision;
            document.getElementById('Emision').value = fechaFormateadaEmision;
    
            // Sumar 30 días para la fecha de expiración
            fechaEmision.setDate(fechaEmision.getDate() + 30);
            var mesExpiracion = fechaEmision.getMonth() + 1;
            var diaExpiracion = fechaEmision.getDate();
            var añoExpiracion = fechaEmision.getFullYear();
    
            // Formateo de la nueva fecha
            if (diaExpiracion < 10) diaExpiracion = '0' + diaExpiracion;
            if (mesExpiracion < 10) mesExpiracion = '0' + mesExpiracion;
    
            var fechaFormateadaExpiracion = añoExpiracion + '-' + mesExpiracion + '-' + diaExpiracion;
            document.getElementById('Expiracion').value = fechaFormateadaExpiracion;
        };
