using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Prestlan.Models;
using IBD.Web;

namespace Prestlan.Utilidades {
    public class Error {
        public static string getError(TiposDeError code) {
            string error = "";
            switch (code) {
                case TiposDeError.DOCUMENTO_NO_EXISTE:
                    error = "errordocnoexiste";
                    break;
                case TiposDeError.DOCUMENTO_CADUCADO:
                    error = "errordoccaducado";
                    break;
                case TiposDeError.DOCUMENTO_YA_CADUCADO:
                    error = "errordocyacaducado";
                    break;
                case TiposDeError.DOCUMENTO_SIN_FECHA_CADUCIDAD:
                    error = "errornofechacaducidad";
                    break;
                case TiposDeError.DOCUMENTO_NO_EN_REVISION:
                    error = "errornoenrevision";
                    break;
                case TiposDeError.DOCUMENTO_NO_ES_BORRADOR:
                    error = "errornoenborrador";
                    break;
                case TiposDeError.DOCUMENTO_NO_PUBLICADO:
                    error = "errornopublicado";
                    break;
                case TiposDeError.DOCUMENTO_AUN_NO_CADUCADO:
                    error = "errordocnocaducado";
                    break;
                case TiposDeError.DOCUMENTO_YA_VALIDADO:
                    error = "errordocyavalidado";
                    break;
                case TiposDeError.USUARIO_NO_AUTORIZADO:
                    error = "errornopermisos";
                    break;
                default:
                    error = "errordesconocido";
                    break;
            }
            return Traduce.getTG(error);
        }
    }
}