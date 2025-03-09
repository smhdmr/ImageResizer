# ImageResizer

**ImageResizer** is a showcase project demonstrating a **scalable and modular backend service** for image processing.  
It receives an image from the user, resizes it to the requested dimensions, and sends a **secure download link** via email.  

&nbsp;&nbsp;

## 🚀 Technologies & Features  

The solution is designed with modern **CQRS** and **event-driven** patterns, incorporating:  

- **MediatR** – Modularizing operations using the CQRS pattern.  
- **RabbitMQ** – Message broker for decoupling Web API and Worker service logic.  
- **MassTransit** – Abstraction layer over RabbitMQ for easier message handling.  
- **SkiaSharp** – High-performance image manipulation library.  
- **AWS S3** – Storage for resized images with **pre-signed URL** generation.  
- **SMTP** – Email service for delivering image download links.  
- **log4net** – Logging solution for structured and persistent logging.  

&nbsp;&nbsp;&nbsp;

## 🏗️ Solution Architecture  

The project consists of **8 modular projects**, ensuring clean separation of concerns:  

### 📌 ImageResizer.WebAPI  
➡️ **Entry point** for handling API requests. Contains controllers, request-response models, and dependency configurations.  

&nbsp;

### 📌 ImageResizer.Client.Application  
➡️ Implements **MediatR commands and handlers** used by the Web API for processing image requests.  

&nbsp;

### 📌 ImageResizer.Worker  
➡️ Background **worker service** that processes image resizing tasks and sends emails.  

&nbsp;

### 📌 ImageResizer.Worker.Application  
➡️ Contains **MediatR consumers** and other services to handle image processing within the worker.  

&nbsp;

### 📌 ImageResizer.Worker.Messaging  
➡️ Defines **RabbitMQ message contracts** for event-driven communication between Web API and Worker.  

&nbsp;

### 📌 ImageResizer.Shared.Application  
➡️ Contains **shared business logic** between Web API and Worker services to avoid duplication.  

&nbsp;

### 📌 ImageResizer.Utilities  
➡️ Utility classes and helper methods used throughout the solution.  

&nbsp;

### 📌 ImageResizer.DataAccess *(Future Use)*  
➡️ Reserved for **database and Redis** operations in future enhancements.  

&nbsp;&nbsp;&nbsp;

## 🔮 Upcoming Features  
- 🌐 **Angular Client** – A web UI for interacting with the API directly from the browser.

&nbsp;&nbsp;
