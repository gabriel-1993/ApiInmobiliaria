# App Mobile para Gimnasio

**Autor:** Hugo Gabriel Torrez  
**Fecha:** [dd/mm/aaaa]

## Tabla de Contenidos
1. [Introducción](#introducción)
2. [Justificación](#justificación)
3. [Objetivo general del proyecto](#objetivo-general-del-proyecto)
4. [Objetivos específicos del proyecto](#objetivos-específicos-del-proyecto)
5. [Objetivo general del sistema](#objetivo-general-del-sistema)
6. [Límite](#límite)
7. [Alcance](#alcance)
8. [No Contemplado](#no-contemplado)
9. [Tecnologías](#tecnologías)
10. [Competencia](#competencia)
11. [Listado de Requerimientos Funcionales](#listado-de-requerimientos-funcionales)
12. [Listado de Requerimientos No Funcionales](#listado-de-requerimientos-no-funcionales)
13. [Desarrollo del Prototipo](#desarrollo-del-prototipo)
14. [Análisis y Diseño](#análisis-y-diseño)
15. [Diagrama de Casos de Uso más Relevantes](#diagrama-de-casos-de-uso-más-relevantes)
16. [Diagrama de Base de Datos (BDD) MySQL](#diagrama-de-base-de-datos-bdd-mysql)
17. [Interfaz Gráfica](#interfaz-gráfica)
18. [Bibliografía](#bibliografía)
19. [Anexo I](#anexo-i)

---

### 1. Introducción
Este proyecto es el trabajo integrador para la materia **Laboratorio 3** de la ULP, con un objetivo doble: desarrollarlo para cumplir con los requisitos académicos y utilizarlo en la práctica dentro de un box de CrossFit. La intención es que el gimnasio cuente con una app personalizada que permita a sus usuarios gestionar reservas de clases y disfrutar de funciones adicionales que mejoren su experiencia general.

### 2. Justificación
Actualmente, el gimnasio utiliza una app genérica no personalizada. Este proyecto busca mejorar la gestión de reservas y añadir una sección en la pantalla de inicio de la app que ofrezca contenido útil y específico para la comunidad del gimnasio. La personalización añadirá identidad propia a la app y aumentará su valor percibido por los usuarios.

### 3. Objetivo General del Proyecto
Crear una aplicación móvil personalizada para el gimnasio, que incluya un sistema de reservas optimizado y contenido adicional para fomentar una comunidad más conectada y comprometida. Se busca ofrecer material informativo sobre técnicas adecuadas de ejercicio y encuestas para optimizar la planificación de rutinas y servicios.

### 4. Formulación de la Problemática
**¿Cómo mejorar la gestión de reservas y la experiencia general de los usuarios en el gimnasio a través de una aplicación móvil personalizada?**

### 5. Justificación de la Importancia del Proyecto
El proyecto facilita la administración diaria del gimnasio y mejora la experiencia del usuario mediante una app personalizada. Los clientes disfrutarán de una interfaz amigable y útil, y la app reforzará la identidad del gimnasio, fortaleciendo la conexión con su comunidad de usuarios.

### 6. Límite
La propuesta se centra en implementar un sistema de reservas para las clases del gimnasio, con un diseño intuitivo y características exclusivas que aporten valor agregado. La app será una extensión del box, conectando mejor a los usuarios con la comunidad y ofreciendo funcionalidades adicionales que la diferencien de otras aplicaciones de gimnasios.

### 7. Alcance
La app ofrecerá una experiencia de usuario intuitiva y minimalista, adecuada para pantallas móviles. Permitirá a los usuarios gestionar reservas y créditos de manera eficiente con una base de datos optimizada. Se incluirán funciones adicionales como acceso a contenido informativo sobre técnicas de ejercicios y participación en encuestas.

### 8. No Contemplado
En la primera versión, la app no incluirá integración con Mercado Pago ni funciones secundarias adicionales. Estas características podrían desarrollarse en versiones futuras.

### 9. Tecnologías
Para el desarrollo del proyecto se utilizarán:
- **Android (Java)** para la programación de la app.
- **.NET con Entity Framework** para el backend.
- **MySQL** para la base de datos.
Estas herramientas crearán un sistema seguro, robusto y escalable.

### 10. Competencia
Actualmente, el gimnasio utiliza una app genérica compartida con otros gimnasios, sin personalización ni integración con métodos de pago. Mi aplicación ofrecerá una solución diferenciada y personalizada, con el objetivo de agregar funcionalidades como pagos en línea en futuras versiones.

### 11. Listado de Requerimientos Funcionales
- Registro y gestión de usuarios
- Recuperación de contraseña
- Administración de perfil de usuario
- Sistema de reservas y administración de turnos

### 12. Listado de Requerimientos No Funcionales
La app deberá cumplir con los siguientes requerimientos no funcionales:
- Compatibilidad con dispositivos Android, incluidas versiones antiguas
- Implementación de prácticas de seguridad en el código y base de datos
- Desempeño optimizado para una experiencia fluida
- Interfaz amigable, intuitiva y minimalista

### 13. Desarrollo del Prototipo
El prototipo inicial se desarrollará con pantallas representativas de las funciones principales (inicio de sesión, registro, restablecimiento de contraseña, pantalla de inicio, gestión de reservas y perfil de usuario). Se crearán prototipos de baja y alta fidelidad y se realizarán revisiones iterativas para mejorar la usabilidad.

### 14. Análisis y Diseño
La arquitectura será cliente-servidor, con la app móvil como cliente que interactúa con una API RESTful en .NET. La API gestionará la lógica de negocio y se conectará a una base de datos MySQL para almacenar usuarios, clases y reservas. La interfaz de la app seguirá un diseño minimalista y amigable.

### 15. Diagrama de Casos de Uso más Relevantes
Los casos de uso principales incluyen:
- **Registro e inicio de sesión**: Registro e inicio de sesión seguro para los usuarios.
- **Gestión de reservas**: Consulta de disponibilidad de clases y reservas/cancelaciones.
- **Visualización de contenido informativo**: Acceso a contenido relevante en la pantalla de inicio.

### 16. Diagrama de Base de Datos (BDD) MySQL
Para ver el diagrama de la base de datos, consulta el archivo `CapturaBDD` en el repositorio.

### 17. Interfaz Gráfica
- **Pantalla de inicio**: Acceso a reservas, perfil y contenido relevante.
- **Pantalla de reservas**: Listado de clases disponibles y opciones para reservar o cancelar.
- **Perfil de usuario**: Visualización y actualización de la información personal del usuario.
La navegación será intuitiva, con un menú en la parte inferior o superior de la pantalla para facilitar el acceso a las funciones.

---

### 18. Bibliografía

### 19. Anexo I

---

¡Gracias por leer! 
