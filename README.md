# Prueba Técnica - Cat Gif App

## Requisitos para correr el proyecto

- .NET 8 SDK instalado (esencial para el backend)  
- Visual Studio 2022 o superior, con ASP.NET instalado  
- Node.js (mínimo versión 16) para el frontend  
- SQL Server funcionando (Express o cualquier otra edición, local o remoto)  

---

## Cómo ejecutar el backend (API)

1. Abrir el proyecto backend en Visual Studio.

2. En el archivo `appsettings.json`, configurar la cadena de conexión a SQL Server. Ejemplo:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_PC\SQLEXPRESS;Database=CatGifDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Abrir la **consola de NuGet** en Visual Studio (Tools > NuGet Package Manager > Package Manager Console).

4. Ejecutar los siguientes comandos para crear la base de datos y las tablas necesarias:

   ```
   Add-Migration InitialCreate
   Update-Database
   ```

5. Ejecutar el backend (F5 en Visual Studio).  
   El proyecto estará disponible en `http://localhost:5091`.  
   *Nota*: Usar la URL HTTP, no HTTPS, para evitar problemas de certificado.

6. Probar la API en el navegador:  
   [http://localhost:5091/api/fact](http://localhost:5091/api/fact)  
   Si se recibe un JSON con un hecho de gatos, está funcionando correctamente.

---

## Cómo ejecutar el frontend (React)

1. Abrir una consola/terminal en la carpeta `Frontend/cat-gif-frontend`.

2. Ejecutar:

   ```
   npm install
   ```

   Esto instalará todas las dependencias del frontend.

3. Luego, iniciar el frontend con:

   ```
   npm start
   ```

4. El frontend se abrirá automáticamente en `http://localhost:3000`.

---

## Cosas importantes a tener en cuenta

- El frontend está configurado para comunicarse con el backend en `http://localhost:5091/api`.  
  Si se modifica el puerto del backend, también se debe actualizar en `src/services/api.ts`.
- El backend está configurado para aceptar peticiones desde cualquier origen (CORS).
- El botón **"Refrescar GIF"** solo actualiza el GIF, sin cambiar el texto del hecho de gatos.
- La tabla de historial es **responsive** y se adapta tanto a móviles como a escritorio.

---

## Resumen rápido de comandos

| Acción                        | Comando / Acción                                         |
|-------------------------------|---------------------------------------------------------|
| Crear base y tablas           | `Add-Migration InitialCreate` + `Update-Database` (NuGet Console) |
| Ejecutar backend              | F5 en Visual Studio                                     |
| Instalar dependencias frontend| `npm install`                                           |
| Ejecutar frontend             | `npm start`                                             |

---

## Problemas comunes y tips

- Si aparece un error de certificado HTTPS, usar la URL `http://localhost:5091` en vez de `https`.
- Si el frontend no carga los GIFs o muestra error de CORS, verificar que el backend esté ejecutándose y la URL base sea correcta.