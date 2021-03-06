﻿namespace Simple.Owin.Static
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Default MIME type resolver.
    /// </summary>
    public class MimeTypeResolver : IMimeTypeResolver
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static readonly IMimeTypeResolver Instance = new MimeTypeResolver();

        private MimeTypeResolver()
        {
            
        }

        /// <summary>
        /// Gives the MIME type for a full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The MIME type, e.g. <c>text/html</c>
        /// </returns>
        public string ForFile(string path)
        {
            return string.IsNullOrWhiteSpace(path)
                ? Default
                : ForExtension(Path.GetExtension(path));
        }

        /// <summary>
        /// Gives the MIME type for a full path.
        /// </summary>
        /// <param name="extension">The extension, prefixed with a period.</param>
        /// <returns>
        /// The MIME type, e.g. <c>text/html</c>
        /// </returns>
        public string ForExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension)) return Default;
            string mimeType;
            return Lookup.TryGetValue(extension, out mimeType)
                ? mimeType
                : Default;
        }

        private const string Default = "application/octet-stream";

        private static readonly Dictionary<string, string> Lookup = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            {".3gp", "video/3gpp"},
            {".3g2", "video/3gpp2"},
            {".7z", "application/x-7z-compressed"},
            {".abw", "application/x-abiword"},
            {".ace", "application/x-ace-compressed"},
            {".adp", "audio/adpcm"},
            {".aab", "application/x-authorware-bin"},
            {".aam", "application/x-authorware-map"},
            {".aas", "application/x-authorware-seg"},
            {".swf", "application/x-shockwave-flash"},
            {".pdf", "application/pdf"},
            {".dir", "application/x-director"},
            {".aac", "audio/x-aac"},
            {".aw", "application/applixware"},
            {".s", "text/x-asm"},
            {".atomcat", "application/atomcat+xml"},
            {".atomsvc", "application/atomsvc+xml"},
            {".atom", "application/atom+xml"},
            {".ac", "application/pkix-attr-cert"},
            {".aif", "audio/x-aiff"},
            {".avi", "video/x-msvideo"},
            {".dxf", "image/vnd.dxf"},
            {".dwf", "model/vnd.dwf"},
            {".par", "text/plain-bas"},
            {".bcpio", "application/x-bcpio"},
            {".bin", "application/octet-stream"},
            {".bmp", "image/bmp"},
            {".torrent", "application/x-bittorrent"},
            {".sh", "application/x-sh"},
            {".btif", "image/prs.btif"},
            {".bz", "application/x-bzip"},
            {".bz2", "application/x-bzip2"},
            {".csh", "application/x-csh"},
            {".c", "text/x-c"},
            {".css", "text/css"},
            {".cdx", "chemical/x-cdx"},
            {".cml", "chemical/x-cml"},
            {".csml", "chemical/x-csml"},
            {".sub", "image/vnd.dvb.subtitle"},
            {".cdmia", "application/cdmi-capability"},
            {".cdmic", "application/cdmi-container"},
            {".cdmid", "application/cdmi-domain"},
            {".cdmio", "application/cdmi-object"},
            {".cdmiq", "application/cdmi-queue"},
            {".ras", "image/x-cmu-raster"},
            {".dae", "model/vnd.collada+xml"},
            {".csv", "text/csv"},
            {".cpt", "application/mac-compactpro"},
            {".cgm", "image/cgm"},
            {".ice", "x-conference/x-cooltalk"},
            {".cmx", "image/x-cmx"},
            {".cpio", "application/x-cpio"},
            {".cif", "chemical/x-cif"},
            {".cmdf", "chemical/x-cmdf"},
            {".cu", "application/cu-seeme"},
            {".cww", "application/prs.cww"},
            {".curl", "text/vnd.curl"},
            {".dcurl", "text/vnd.curl.dcurl"},
            {".mcurl", "text/vnd.curl.mcurl"},
            {".scurl", "text/vnd.curl.scurl"},
            {".dssc", "application/dssc+der"},
            {".xdssc", "application/dssc+xml"},
            {".deb", "application/x-debian-package"},
            {".uva", "audio/vnd.dece.audio"},
            {".uvi", "image/vnd.dece.graphic"},
            {".uvh", "video/vnd.dece.hd"},
            {".uvm", "video/vnd.dece.mobile"},
            {".uvu", "video/vnd.uvvu.mp4"},
            {".uvp", "video/vnd.dece.pd"},
            {".uvs", "video/vnd.dece.sd"},
            {".uvv", "video/vnd.dece.video"},
            {".dvi", "application/x-dvi"},
            {".dtb", "application/x-dtbook+xml"},
            {".res", "application/x-dtbresource+xml"},
            {".eol", "audio/vnd.digital-winds"},
            {".djvu", "image/vnd.djvu"},
            {".dtd", "application/xml-dtd"},
            {".wad", "application/x-doom"},
            {".dra", "audio/vnd.dra"},
            {".dts", "audio/vnd.dts"},
            {".dtshd", "audio/vnd.dts.hd"},
            {".dwg", "image/vnd.dwg"},
            {".es", "application/ecmascript"},
            {".mmr", "image/vnd.fujixerox.edmics-mmr"},
            {".rlc", "image/vnd.fujixerox.edmics-rlc"},
            {".exi", "application/exi"},
            {".epub", "application/epub+zip"},
            {".eml", "message/rfc822"},
            {".xif", "image/vnd.xiff"},
            {".emma", "application/emma+xml"},
            {".fst", "image/vnd.fst"},
            {".fvt", "video/vnd.fvt"},
            {".fbs", "image/vnd.fastbidsheet"},
            {".f4v", "video/x-f4v"},
            {".flv", "video/x-flv"},
            {".fpx", "image/vnd.fpx"},
            {".npx", "image/vnd.net-fpx"},
            {".flx", "text/vnd.fmi.flexstor"},
            {".fli", "video/x-fli"},
            {".f", "text/x-fortran"},
            {".fh", "image/x-freehand"},
            {".spl", "application/x-futuresplash"},
            {".g3", "image/g3fax"},
            {".gtw", "model/vnd.gtw"},
            {".gdl", "model/vnd.gdl"},
            {".gsf", "application/x-font-ghostscript"},
            {".bdf", "application/x-font-bdf"},
            {".gtar", "application/x-gtar"},
            {".texinfo", "application/x-texinfo"},
            {".gnumeric", "application/x-gnumeric"},
            {".gif", "image/gif"},
            {".gv", "text/vnd.graphviz"},
            {".h261", "video/h261"},
            {".h263", "video/h263"},
            {".h264", "video/h264"},
            {".hdf", "application/x-hdf"},
            {".rip", "audio/vnd.rip"},
            {".stk", "application/hyperstudio"},
            {".html", "text/html"},
            {".htm", "text/html"},
            {".ics", "text/calendar"},
            {".ico", "image/x-icon"},
            {".ief", "image/ief"},
            {".rif", "application/reginfo+xml"},
            {".3dml", "text/vnd.in3d.3dml"},
            {".spot", "text/vnd.in3d.spot"},
            {".igs", "model/iges"},
            {".ipfix", "application/ipfix"},
            {".cer", "application/pkix-cert"},
            {".pki", "application/pkixcmp"},
            {".crl", "application/pkix-crl"},
            {".pkipath", "application/pkix-pkipath"},
            {".jad", "text/vnd.sun.j2me.app-descriptor"},
            {".jar", "application/java-archive"},
            {".class", "application/java-vm"},
            {".jnlp", "application/x-java-jnlp-file"},
            {".ser", "application/java-serialized-object"},
            {".java", "text/x-java-source,java"},
            {".js", "application/javascript"},
            {".json", "application/json"},
            {".jpm", "video/jpm"},
            {".jpeg", "image/jpeg"},
            {".jpg", "image/jpeg"},
            {".jpgv", "video/jpeg"},
            {".latex", "application/x-latex"},
            {".lvp", "audio/vnd.lucent.voice"},
            {".m3u", "audio/x-mpegurl"},
            {".m4v", "video/x-m4v"},
            {".hqx", "application/mac-binhex40"},
            {".mrc", "application/marc"},
            {".mrcx", "application/marcxml+xml"},
            {".mxf", "application/mxf"},
            {".ma", "application/mathematica"},
            {".mathml", "application/mathml+xml"},
            {".mbox", "application/mbox"},
            {".mscml", "application/mediaservercontrol+xml"},
            {".msh", "model/mesh"},
            {".mads", "application/mads+xml"},
            {".mets", "application/mets+xml"},
            {".mods", "application/mods+xml"},
            {".meta4", "application/metalink4+xml"},
            {".mdb", "application/x-msaccess"},
            {".asf", "video/x-ms-asf"},
            {".exe", "application/x-msdownload"},
            {".application", "application/x-ms-application"},
            {".clp", "application/x-msclip"},
            {".mdi", "image/vnd.ms-modi"},
            {".crd", "application/x-mscardfile"},
            {".mvb", "application/x-msmediaview"},
            {".mny", "application/x-msmoney"},
            {".obd", "application/x-msbinder"},
            {".onetoc", "application/onenote"},
            {".pya", "audio/vnd.ms-playready.media.pya"},
            {".pyv", "video/vnd.ms-playready.media.pyv"},
            {".pub", "application/x-mspublisher"},
            {".scd", "application/x-msschedule"},
            {".xap", "application/x-silverlight-app"},
            {".wm", "video/x-ms-wm"},
            {".wma", "audio/x-ms-wma"},
            {".wax", "audio/x-ms-wax"},
            {".wmx", "video/x-ms-wmx"},
            {".wmd", "application/x-ms-wmd"},
            {".wmz", "application/x-ms-wmz"},
            {".wmv", "video/x-ms-wmv"},
            {".wvx", "video/x-ms-wvx"},
            {".wmf", "application/x-msmetafile"},
            {".trm", "application/x-msterminal"},
            {".doc", "application/msword"},
            {".wri", "application/x-mswrite"},
            {".xbap", "application/x-ms-xbap"},
            {".mid", "audio/midi"},
            {".prc", "application/x-mobipocket-ebook"},
            {".fly", "text/vnd.fly"},
            {".mj2", "video/mj2"},
            {".mpga", "audio/mpeg"},
            {".mxu", "video/vnd.mpegurl"},
            {".mpeg", "video/mpeg"},
            {".m21", "application/mp21"},
            {".mp4a", "audio/mp4"},
            {".mp4", "video/mp4"},
            {".mxml", "application/xv+xml"},
            {".ncx", "application/x-dtbncx+xml"},
            {".nc", "application/x-netcdf"},
            {".n3", "text/n3"},
            {".ecelp4800", "audio/vnd.nuera.ecelp4800"},
            {".ecelp7470", "audio/vnd.nuera.ecelp7470"},
            {".ecelp9600", "audio/vnd.nuera.ecelp9600"},
            {".oda", "application/oda"},
            {".ogx", "application/ogg"},
            {".oga", "audio/ogg"},
            {".ogv", "video/ogg"},
            {".opf", "application/oebps-package+xml"},
            {".weba", "audio/webm"},
            {".webm", "video/webm"},
            {".ktx", "image/ktx"},
            {".otf", "application/x-font-otf"},
            {".p", "text/x-pascal"},
            {".pcx", "image/x-pcx"},
            {".psd", "image/vnd.adobe.photoshop"},
            {".prf", "application/pics-rules"},
            {".pic", "image/x-pict"},
            {".chat", "application/x-chat"},
            {".p10", "application/pkcs10"},
            {".p12", "application/x-pkcs12"},
            {".p7m", "application/pkcs7-mime"},
            {".p7s", "application/pkcs7-signature"},
            {".p7r", "application/x-pkcs7-certreqresp"},
            {".p7b", "application/x-pkcs7-certificates"},
            {".p8", "application/pkcs8"},
            {".pnm", "image/x-portable-anymap"},
            {".pbm", "image/x-portable-bitmap"},
            {".pcf", "application/x-font-pcf"},
            {".pfr", "application/font-tdpfr"},
            {".pgn", "application/x-chess-pgn"},
            {".pgm", "image/x-portable-graymap"},
            {".png", "image/png"},
            {".ppm", "image/x-portable-pixmap"},
            {".pskcxml", "application/pskc+xml"},
            {".ai", "application/postscript"},
            {".pfa", "application/x-font-type1"},
            {".pgp", "application/pgp-signature"},
            {".pls", "application/pls+xml"},
            {".dsc", "text/prs.lines.tag"},
            {".psf", "application/x-font-linux-psf"},
            {".qt", "video/quicktime"},
            {".rar", "application/x-rar-compressed"},
            {".ram", "audio/x-pn-realaudio"},
            {".rmp", "audio/x-pn-realaudio-plugin"},
            {".rsd", "application/rsd+xml"},
            {".rnc", "application/relax-ng-compact-syntax"},
            {".rdf", "application/rdf+xml"},
            {".rtf", "application/rtf"},
            {".rtx", "text/richtext"},
            {".rss", "application/rss+xml"},
            {".shf", "application/shf+xml"},
            {".svg", "image/svg+xml"},
            {".sru", "application/sru+xml"},
            {".setpay", "application/set-payment-initiation"},
            {".setreg", "application/set-registration-initiation"},
            {".snf", "application/x-font-snf"},
            {".spq", "application/scvp-vp-request"},
            {".spp", "application/scvp-vp-response"},
            {".scq", "application/scvp-cv-request"},
            {".scs", "application/scvp-cv-response"},
            {".sdp", "application/sdp"},
            {".etx", "text/x-setext"},
            {".movie", "video/x-sgi-movie"},
            {".tfi", "application/thraud+xml"},
            {".shar", "application/x-shar"},
            {".rgb", "image/x-rgb"},
            {".rq", "application/sparql-query"},
            {".srx", "application/sparql-results+xml"},
            {".gram", "application/srgs"},
            {".grxml", "application/srgs+xml"},
            {".ssml", "application/ssml+xml"},
            {".sgml", "text/sgml"},
            {".sit", "application/x-stuffit"},
            {".sitx", "application/x-stuffitx"},
            {".au", "audio/basic"},
            {".smi", "application/smil+xml"},
            {".sv4cpio", "application/x-sv4cpio"},
            {".sv4crc", "application/x-sv4crc"},
            {".sbml", "application/sbml+xml"},
            {".tsv", "text/tab-separated-values"},
            {".tiff", "image/tiff"},
            {".tar", "application/x-tar"},
            {".tcl", "application/x-tcl"},
            {".tex", "application/x-tex"},
            {".tfm", "application/x-tex-tfm"},
            {".tei", "application/tei+xml"},
            {".txt", "text/plain"},
            {".tsd", "application/timestamped-data"},
            {".t", "text/troff"},
            {".ttf", "application/x-font-ttf"},
            {".ttl", "text/turtle"},
            {".uri", "text/uri-list"},
            {".ustar", "application/x-ustar"},
            {".uu", "text/x-uuencode"},
            {".vcs", "text/x-vcalendar"},
            {".vcf", "text/x-vcard"},
            {".vcd", "application/x-cdlink"},
            {".wrl", "model/vrml"},
            {".mts", "model/vnd.mts"},
            {".vtu", "model/vnd.vtu"},
            {".viv", "video/vnd.vivo"},
            {".ccxml", "application/ccxml+xml,"},
            {".vxml", "application/voicexml+xml"},
            {".src", "application/x-wais-source"},
            {".wbmp", "image/vnd.wap.wbmp"},
            {".wav", "audio/x-wav"},
            {".davmount", "application/davmount+xml"},
            {".woff", "application/x-font-woff"},
            {".wspolicy", "application/wspolicy+xml"},
            {".webp", "image/webp"},
            {".wgt", "application/widget"},
            {".hlp", "application/winhlp"},
            {".wml", "text/vnd.wap.wml"},
            {".wmls", "text/vnd.wap.wmlscript"},
            {".wsdl", "application/wsdl+xml"},
            {".xbm", "image/x-xbitmap"},
            {".xpm", "image/x-xpixmap"},
            {".xwd", "image/x-xwindowdump"},
            {".der", "application/x-x509-ca-cert"},
            {".fig", "application/x-xfig"},
            {".xhtml", "application/xhtml+xml"},
            {".xml", "application/xml"},
            {".xdf", "application/xcap-diff+xml"},
            {".xenc", "application/xenc+xml"},
            {".xer", "application/patch-ops-error+xml"},
            {".rl", "application/resource-lists+xml"},
            {".rs", "application/rls-services+xml"},
            {".rld", "application/resource-lists-diff+xml"},
            {".xslt", "application/xslt+xml"},
            {".xop", "application/xop+xml"},
            {".xpi", "application/x-xpinstall"},
            {".xspf", "application/xspf+xml"},
            {".xyz", "chemical/x-xyz"},
            {".yang", "application/yang"},
            {".yin", "application/yin+xml"},
            {".zip", "application/zip"}

        };
    }
}