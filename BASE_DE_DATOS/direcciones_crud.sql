-- FUNCTION: api_direcciones.direcciones_crud(integer, integer, character varying, character varying, character varying, integer, character varying, boolean, timestamp with time zone, timestamp with time zone)

-- DROP FUNCTION IF EXISTS api_direcciones.direcciones_crud(integer, integer, character varying, character varying, character varying, integer, character varying, boolean, timestamp with time zone, timestamp with time zone);

CREATE OR REPLACE FUNCTION api_direcciones.direcciones_crud(
	p_opciones integer,
	p_id_direcciones integer DEFAULT NULL::integer,
	p_calle character varying DEFAULT NULL::character varying,
	p_colonia character varying DEFAULT NULL::character varying,
	p_municipio character varying DEFAULT NULL::character varying,
	p_numero integer DEFAULT NULL::integer,
	p_cp character varying DEFAULT NULL::character varying,
	p_activo boolean DEFAULT NULL::boolean,
	p_fecha_modificacion timestamp with time zone DEFAULT NULL::timestamp with time zone,
	p_fecha_creacion timestamp with time zone DEFAULT NULL::timestamp with time zone)
    RETURNS TABLE(id integer, calle character varying, colonia character varying, municipio character varying, numero integer, cp character varying, activo boolean, fecha_modificacion timestamp with time zone, fecha_creacion timestamp with time zone) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
declare
    new_row          int     := null;
    sql_success      boolean := true;
    sql_no_error     varchar := '';
    sql_msg_error    varchar := '';
    sql_detail_error varchar := '';
    sql_ex_error     text    := '';
BEGIN
    -- Opción 1: Insertar un nuevo registro 
    IF p_opciones = 1 THEN
        INSERT INTO api_direcciones.td_direcciones (calle,colonia, municipio, numero, cp, activo, fecha_modificacion, fecha_creacion)
        VALUES (p_calle,p_colonia,p_municipio,p_numero, p_cp, true,  now(), now());
        
        new_row :=currval('api_direcciones.td_direcciones_id_seq');
        RAISE NOTICE 'Registro de  consulta con numero %',new_row;

        return query select new_row,p_calle,p_colonia,p_municipio,p_numero, p_cp, p_activo, now(), now();	
        
    -- Opción 2: Consultar registros por id 
    ELSIF p_opciones = 2 THEN
		IF p_id_direcciones IS NOT NULL THEN

		
			RETURN QUERY
	        SELECT td.id, td.calle,td.colonia, td.municipio, td.numero, td.cp, td.activo, td.fecha_modificacion, td.fecha_creacion
	        FROM api_direcciones.td_direcciones td
	        WHERE td.id = p_id_direcciones and td.activo=true;
		ELSE
        	RAISE EXCEPTION 'Se Requiere un ID Para La Consulta de Registro:%', p_id_direcciones;
		
		END IF;

        

    -- Opción 3: Actualizar un registro existente
    ELSIF p_opciones = 3 THEN
		IF p_id_direcciones IS NOT NULL THEN

	        UPDATE api_direcciones.td_direcciones t
	        SET 
	            calle = p_calle,
				colonia = p_colonia,
				municipio = p_municipio,
				numero = p_numero,
	            cp = p_cp,
	            activo = p_activo,
	            fecha_modificacion =  now()
	        WHERE t.id = p_id_direcciones;
	        return query select p_id_direcciones,p_calle,p_colonia,p_municipio,p_numero, p_cp, p_activo, now(), now();	

		ELSE
        	RAISE EXCEPTION 'Se Requiere un ID Para La Actualizar Registro:%', p_id_direcciones;
		
		END IF;

    -- Opción 4: Eliminar un registro
    ELSIF p_opciones = 4 THEN
		IF p_id_direcciones IS NOT NULL THEN
	        UPDATE api_direcciones.td_direcciones t
	        SET 
	            activo = false,
	            fecha_modificacion = now()
	        WHERE t.id = p_id_direcciones;
		return query select p_id_direcciones,p_calle,p_colonia,p_municipio,p_numero, p_cp, activo, now(), now();	

		ELSE
        	RAISE EXCEPTION 'Se Requiere un ID Para Eliminar Registro:%', p_id_direcciones;
		END IF;

	-- Opción 5: Consultar todos los registros 
	ELSIF p_opciones = 5 THEN
		RETURN QUERY
	        SELECT td.id, td.calle,td.colonia, td.municipio, td.numero, td.cp, td.activo, td.fecha_modificacion, td.fecha_creacion
	        FROM api_direcciones.td_direcciones td where td.activo=true;

    ELSE
        RAISE EXCEPTION 'Opción no válida: %', p_opciones;
    END IF;
	

END;
$BODY$;

ALTER FUNCTION api_direcciones.direcciones_crud(integer, integer, character varying, character varying, character varying, integer, character varying, boolean, timestamp with time zone, timestamp with time zone)
    OWNER TO postgres;
