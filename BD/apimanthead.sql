--------------------------------------------------------
-- Archivo creado  - lunes-noviembre-23-2020   
--------------------------------------------------------
--------------------------------------------------------
--  DDL for Package PKG_API_MANT
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "APIINST"."PKG_API_MANT" AS 
PROCEDURE PRC_TAREA_INSERTAR (
X_ID_TIPO IN apiinst.tareas.ID_TIPO_TAREA%TYPE,
X_USER IN apiinst.mlogin.USUARIO%TYPE,
X_VEL IN apiinst.methodlike.VELOCIDAD%TYPE,
X_CANT IN apiinst.mlikemanypost.CANTLIKE%TYPE,
X_USERLIST IN apiinst.methodlike.USUARIOS%TYPE,
X_ERROR OUT SYS_REFCURSOR
);

--Procedimiento para Insetar Valores en la Tabla Reportes_Mensajes(REPORT_MESS)
PROCEDURE PRC_INSERT_REPORT_MESS (
X_THREAD_ID IN apiinst.report_mess.THREAD_ID%TYPE,
X_ITEM_ID IN apiinst.report_mess.ITEM_ID%TYPE,
X_CLIENTE_ID IN apiinst.report_mess.CLIENTE_ID%TYPE,
X_CANT_TOTAL IN apiinst.report_mess.CANT_TOTAL%TYPE,
X_CANT_ENV IN apiinst.report_mess.CANT_ENV%TYPE,
X_LIST_IDS IN apiinst.report_mess.LIST_IDS%TYPE,
X_ERROR OUT SYS_REFCURSOR
);


--Procedimiento par actualizar la tabla de los reportes de mensajes
PROCEDURE PRC_UPDATE_REPORT_MESS (
X_THREAD_ID IN apiinst.report_mess.THREAD_ID%TYPE,
X_CLIENTE_ID IN apiinst.report_mess.CLIENTE_ID%TYPE,
X_CANT_VISTOS IN apiinst.report_mess.CANT_VISTOS%TYPE,
X_CANT_REACC IN apiinst.report_mess.CANT_REACC%TYPE,
X_ERROR OUT SYS_REFCURSOR
);

--Procedimiento para insertar la tarea de purificar cuentas
PROCEDURE PRC_PURIFICADOR_INSERTAR
(X_USUARIO IN apiinst.purificador.USUARIO%TYPE,
X_USUARIOS IN apiinst.purificador.USUARIOS%TYPE,
X_ID_TIPO IN apiinst.tareas.ID_TIPO_TAREA%TYPE,
X_ERROR OUT SYS_REFCURSOR
);

PROCEDURE PRC_MASSENDING_INSERTAR(
X_IDMLOGIN IN apiinst.massending.IDMLOGIN%TYPE,
X_USUARIOS IN apiinst.massending.USUARIOS%TYPE,
X_TEXTO IN apiinst.massending.TEXTO%TYPE,
X_ID_TIPO IN apiinst.tareas.ID_TIPO_TAREA%TYPE,
X_ERROR OUT SYS_REFCURSOR
);
END PKG_API_MANT;

/
