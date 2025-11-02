# 🏪 Sneaker Shop Management System

A complete, professional sneaker shop management system built with C# Windows Forms and Supabase PostgreSQL. Features full CRUD operations, inventory management, sales processing, and role-based authentication.

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Supabase](https://img.shields.io/badge/Supabase-3ECF8E?style=for-the-badge&logo=supabase&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-316192?style=for-the-badge&logo=postgresql&logoColor=white)

## ✨ Features

### 🔐 Authentication & Security
- **Role-based access control** (Admin/Staff)
- **BCrypt password hashing** for secure authentication
- **Session management** with secure login/logout

### 👥 Customer Management  
- **Complete CRUD operations** for customer data
- **Purchase history tracking** with detailed sales records
- **Search and filter functionality**
- **Customer analytics** (total purchases, spending)

### 👟 Product & Inventory
- **Sneaker management** with brands, sizes, colors
- **Real-time inventory tracking** with stock status
- **Automatic low stock alerts**
- **Product categorization** by brands

### 💰 Sales & Point of Sale
- **Complete sales transaction system**
- **Shopping cart functionality** 
- **Automated inventory updates** after sales
- **Professional receipt generation**
- **Staff commission tracking**

### 🏷️ Brand Management
- **Brand catalog management**
- **Product organization** by brands
- **CRUD operations** for brand data

### 👨‍💼 Staff Management
- **Admin-only staff management**
- **Role assignment** (Admin/Staff)
- **Secure password generation**
- **Staff activity tracking**

## 🛠️ Tech Stack

- **Frontend**: C# Windows Forms (.NET Framework)
- **Backend**: Supabase (PostgreSQL)
- **Authentication**: BCrypt.Net-Next
- **ORM**: Postgrest (Supabase .NET client)
- **Architecture**: MVC Pattern with Services Layer

## 📁 Project Structure

SneakerShop/
├── Forms/ # Windows Forms UI
│ ├── MainMenu.cs # Navigation system
│ ├── LoginForm.cs # Authentication
│ ├── CustomerForm.cs # Customer management + purchase history
│ ├── ProductForm.cs # Sneaker CRUD operations
│ ├── InventoryForm.cs # Stock management
│ ├── SaleForm.cs # Point of Sale system
│ ├── BrandForm.cs # Brand management
│ └── StaffForm.cs # Staff management (Admin only) ✅
├── Models/ # Data models
│ ├── Customer.cs # Customer entity
│ ├── Sneaker.cs # Product entity
│ ├── Brand.cs # Brand entity
│ ├── Sale.cs # Sales transactions
│ ├── SaleDetail.cs # Sales line items
│ └── User.cs # Staff/User accounts
├── Services/ # Business logic & data access
│ ├── DatabaseService.cs # Database operations
│ ├── SupabaseClient.cs # Supabase connection
│ └── AuthenticationService.cs # Auth logic
└── Program.cs # Application entry point

## 🚀 Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2 or later
- Supabase account

### Installation

1. **Clone the repository**
   ```bash
   git clone (https://github.com/liuKick/sneakers-inventory-system)
   Database Setup

2. The project is already configured with Supabase

Database URL: https://dgnnhpphgkewbgyeohgc.supabase.co

All tables are pre-configured with proper RLS policies

3. Build and Run

Open SneakerShop.sln in Visual Studio

Restore NuGet packages

Build the solution (Ctrl+Shift+B)

Run the application (F5)

## Default Login Credentials ##
   Admin: admin / admin123
   Staff: staff / staff123

### 🗄️ Database Schema
The system uses 6 main tables with proper relationships:

sql
-- Main Tables:
users (user_id, username, password, role, status, created_at)
customers (customer_id, name, phone, email, created_at) 
brands (brand_id, brand_name, created_at)
sneakers (sneaker_id, display_id, name, brand_id, size, color, price, stock_quantity, created_at)
sales (sale_id, customer_id, staff_id, date, total_amount, created_at)
sale_details (sale_detail_id, sale_id, sneaker_id, quantity, unit_price, sub_total, created_at)
Database Features:
✅ Row Level Security (RLS) enabled on all tables

✅ Proper foreign key relationships

✅ Auto-generated UUID primary keys

✅ Timestamps for all records

✅ Status tracking for users

🔧 Configuration
The application is pre-configured with your Supabase project:
// In Services/SupabaseClient.cs
var url = "https://dgnnhpphgkewbgyeohgc.supabase.co";
var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImRnbm5ocHBoZ2tld2JneWVvaGdjIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjAzMTE5OTAsImV4cCI6MjA3NTg4Nzk5MH0.TKXuUeMhR3BW6yQu33aJvcYpwV1b-Hio5okHZqno3Kw"; // Already configured



📊 Current Database Status
✅ All tables created and properly structured

✅ RLS policies implemented for security

✅ Sample data ready for testing

✅ Relationships established between tables

✅ Authentication configured with BCrypt

🎯 Key Features Implemented
✅ Full CRUD operations across all modules

✅ Real-time database synchronization with Supabase

✅ Professional UI/UX with consistent design patterns

✅ Error handling and user-friendly messages

✅ Data validation and input sanitization

✅ Role-based security and access control

✅ Purchase history and customer analytics

✅ Inventory management with stock status

✅ Sales reporting and transaction tracking

✅ Staff management with admin controls

👥 Staff Management Features
Admin-only access to staff management

Role-based permissions (Admin/Staff)

Status tracking (Active/Inactive)

Secure password generation with BCrypt

Complete CRUD operations for staff accounts

Professional UI matching other forms

🤝 Contributing
Fork the project

Create your feature branch (git checkout -b feature/AmazingFeature)

Commit your changes (git commit -m 'Add some AmazingFeature')

Push to the branch (git push origin feature/AmazingFeature)

Open a Pull Request

📄 License
This project is licensed under the MIT License - see the LICENSE.md file for details.

🏆 Achievements
8 fully functional forms with professional UI

Complete database integration with Supabase PostgreSQL

Role-based authentication system

Production-ready architecture

Comprehensive error handling

Live Supabase backend with proper security

