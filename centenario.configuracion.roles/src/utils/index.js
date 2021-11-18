export const LISTAS = {
    //PRINCIPAL
    CONFIGURACIONACCESOS: 'Configuracion Accesos',
    //Rentas
    ROLES: 'Roles',
    EJECUTIVOS_MINKA: 'Ejecutivos Minka',
    EJECUTIVOS_RI: 'Ejecutivos Renta Inmobiliaria',
    
};


export const SUBSITIOS = {
    Renta: 'rentas',
};

export const CONSTANTES = {
    CREDITOS_COBRANZAS: 'Creditos y Cobranzas',
    DESARROLLO_URBANO: 'Desarrollo Urbano',
    RENTA_INMOBILIARIA: 'Renta Inmobiliaria'
};

export const removerTildes = (function(){
    const de = '\u00C1\u00C3\u00C0\u00C4\u00C2\u00C9\u00CB\u00C8\u00CA\u00CD\u00CF\u00CC\u00CE\u00D3\u00D6\u00D2\u00D4\u00DA\u00DC\u00D9\u00DB\u00D1\u00C7\u00E1\u00E3\u00E0\u00E4\u00E2\u00E9\u00EB\u00E8\u00EA\u00ED\u00EF\u00EC\u00EE\u00F3\u00F6\u00F2\u00F4\u00FA\u00FC\u00F9\u00FB\u00F1\u00E7',
        a = 'AAAAAEEEEIIIIOOOOUUUUNCaaaaaeeeeiiiioooouuuunc',
        re = /[\u00C1\u00C3\u00C0\u00C4\u00C2\u00C9\u00CB\u00C8\u00CA\u00CD\u00CF\u00CC\u00CE\u00D3\u00D6\u00D2\u00D4\u00DA\u00DC\u00D9\u00DB\u00D1\u00C7\u00E1\u00E3\u00E0\u00E4\u00E2\u00E9\u00EB\u00E8\u00EA\u00ED\u00EF\u00EC\u00EE\u00F3\u00F6\u00F2\u00F4\u00FA\u00FC\u00F9\u00FB\u00F1\u00E7]/ug;
        
    return texto => 
        texto.replace(
            re,
            match => a.charAt(de.indexOf(match))
        );
})();