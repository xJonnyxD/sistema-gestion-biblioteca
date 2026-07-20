# Sistema de Gestión de Biblioteca

Aplicación web desarrollada con ASP.NET Core MVC para la administración de una biblioteca:
catálogo de libros, autores, categorías, usuarios y control de préstamos.

Proyecto correspondiente al módulo **Programación Web II (Frontend)**.

## Tecnologías

| Componente | Versión |
|---|---|
| .NET | 10.0 |
| ASP.NET Core MVC | 10.0 |
| C# | 14 |
| Bootstrap | 5 |
| jQuery | 3.7 |

## Requisitos previos

- SDK de .NET 10.0 o superior
- Visual Studio 2022 Community, o cualquier editor con soporte para C#

## Ejecución

```bash
cd BibliotecaWeb
dotnet restore
dotnet run
```

La aplicación queda disponible en la URL que indica la consola (por defecto `https://localhost:7xxx`).

## Arquitectura

El proyecto implementa el patrón **Modelo–Vista–Controlador (MVC)**:

- **Modelo**: define la estructura de los datos de cada entidad del dominio.
- **Vista**: construye la interfaz que consume el usuario, usando Razor.
- **Controlador**: recibe la petición, arma la información a partir del modelo y la envía a la vista.

Cada módulo del sistema cuenta con su propio modelo, controlador y vista, de modo que la lógica
de una entidad no se mezcla con la de las demás.

## Estructura del proyecto

```
BibliotecaWeb/
├── Controllers/          Controladores, uno por cada módulo del sistema
├── Models/               Clases que representan las entidades del dominio
├── Views/                Vistas Razor (.cshtml) agrupadas por controlador
│   ├── Autores/
│   ├── Categorias/
│   ├── Home/
│   ├── Libros/
│   ├── Prestamos/
│   ├── Usuarios/
│   ├── Shared/           Plantilla, menú principal y vistas compartidas
│   ├── _ViewImports.cshtml   Directivas @using y tag helpers globales
│   └── _ViewStart.cshtml     Layout aplicado por defecto a todas las vistas
├── wwwroot/              Archivos estáticos (CSS, JS, imágenes, librerías)
├── Properties/
│   └── launchSettings.json   Perfiles de ejecución local
├── appsettings.json      Configuración de la aplicación
├── Program.cs            Punto de entrada y configuración del pipeline
└── BibliotecaWeb.csproj  Definición del proyecto y sus dependencias
```

## Controladores

| Controlador | Acción | Ruta | Modelo | Vista | Descripción |
|---|---|---|---|---|---|
| `HomeController` | `Index` | `/` | — | `Views/Home/Index.cshtml` | Página de inicio con accesos a los módulos |
| `HomeController` | `AcercaDe` | `/Home/AcercaDe` | — | `Views/Home/AcercaDe.cshtml` | Información general del sistema |
| `HomeController` | `Error` | `/Home/Error` | `ErrorViewModel` | `Views/Shared/Error.cshtml` | Página de error de la aplicación |
| `LibrosController` | `Index` | `/Libros` | `IEnumerable<Libro>` | `Views/Libros/Index.cshtml` | Catálogo de libros y su disponibilidad |
| `AutoresController` | `Index` | `/Autores` | `IEnumerable<Autor>` | `Views/Autores/Index.cshtml` | Autores registrados y su estado |
| `CategoriasController` | `Index` | `/Categorias` | `IEnumerable<Categoria>` | `Views/Categorias/Index.cshtml` | Categorías de clasificación de libros |
| `UsuariosController` | `Index` | `/Usuarios` | `IEnumerable<Usuario>` | `Views/Usuarios/Index.cshtml` | Usuarios habilitados para préstamos |
| `PrestamosController` | `Index` | `/Prestamos` | `IEnumerable<Prestamo>` | `Views/Prestamos/Index.cshtml` | Registro y control de préstamos |

Cada controlador arma la colección de su entidad y la envía a la vista mediante `return View(modelo)`,
de forma que las vistas trabajan con modelos fuertemente tipados.

## Modelos

| Modelo | Propiedades |
|---|---|
| `Libro` | `Id`, `Titulo`, `Autor`, `Categoria`, `AnioPublicacion`, `Disponibles`, `HayDisponibles` |
| `Autor` | `Id`, `Nombre`, `Apellido`, `Nacionalidad`, `FechaNacimiento`, `Activo` |
| `Categoria` | `Id`, `Nombre`, `Descripcion`, `Activa` |
| `Usuario` | `Id`, `Carne`, `Nombre`, `Apellido`, `Correo`, `Activo` |
| `Prestamo` | `Id`, `Libro`, `Usuario`, `FechaPrestamo`, `FechaDevolucion`, `Estado` |
| `ErrorViewModel` | `RequestId`, `ShowRequestId` |

El estado de un préstamo se representa con la enumeración `EstadoPrestamo`
(`Pendiente`, `Devuelto`, `Atrasado`), lo que evita usar cadenas de texto sueltas
para un valor que solo admite tres posibilidades.

## Vistas compartidas

- **`_Layout.cshtml`** — plantilla común a todas las páginas. Contiene el encabezado, el
  menú, el pie de página y la llamada a `@RenderBody()`, que inserta el contenido de cada vista.
- **`_MenuPrincipal.cshtml`** — vista parcial con el menú de navegación. Al estar separada
  del layout, el menú se modifica en un solo archivo y el cambio se refleja en todo el sitio.

## Menú del sistema

Inicio · Libros · Autores · Categorías · Usuarios · Préstamos · Acerca de

## Estado actual

Los datos se cargan en memoria dentro de cada controlador. La conexión a base de datos
con Entity Framework está prevista para una etapa posterior; cuando se incorpore, únicamente
cambia el origen de los datos y los modelos, vistas y rutas se mantienen sin modificación.
