--------------------------------------------------------
-- Archivo creado  - lunes-noviembre-23-2020   
--------------------------------------------------------
DROP PACKAGE "APIINST"."PKG_API_CONS";
--------------------------------------------------------
--  DDL for Package PKG_API_CONS
--------------------------------------------------------

  CREATE OR REPLACE PACKAGE "APIINST"."PKG_API_CONS" AS 

PROCEDURE PRC_GET_TAREAS(
X_CURSOR OUT SYS_REFCURSOR);
--Procedimiento para obtener tarea Expancion
PROCEDURE PRC_GET_TAREA_EXPANCION(
X_ID_TAREA IN NUMBER,
X_CURSOR OUT SYS_REFCURSOR);

--Procedimiento para obtener tarea de Purificacion
PROCEDURE PRC_GET_TAREA_PURIFICACION
(
X_ID_TAREA IN NUMBER,
X_CURSOR OUT SYS_REFCURSOR
);

PROCEDURE PRC_GET_REPORES_MSJ(
X_CLIENTE_ID IN NUMBER,
X_CURSOR OUT SYS_REFCURSOR);
PROCEDURE PRC_GET_MASS_SENDING(
X_ID_TAREA IN NUMBER,
X_CURSOR OUT SYS_REFCURSOR);

END PKG_API_CONS;

/
