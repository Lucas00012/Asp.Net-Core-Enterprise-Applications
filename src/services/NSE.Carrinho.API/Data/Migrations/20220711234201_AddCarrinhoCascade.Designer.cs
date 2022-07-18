﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSE.Carrinho.API.Data;

#nullable disable

namespace NSE.Carrinho.API.Data.Migrations
{
    [DbContext(typeof(CarrinhoContext))]
    [Migration("20220711234201_AddCarrinhoCascade")]
    partial class AddCarrinhoCascade
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NSE.Carrinho.API.Models.CarrinhoCliente", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ClienteId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Desconto")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ValorTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("VoucherUtilizado")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId")
                        .HasDatabaseName("IDX_Cliente");

                    b.ToTable("CarrinhoCliente");
                });

            modelBuilder.Entity("NSE.Carrinho.API.Models.CarrinhoItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CarrinhoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Imagem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProdutoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CarrinhoId");

                    b.ToTable("CarrinhoItens");
                });

            modelBuilder.Entity("NSE.Carrinho.API.Models.CarrinhoCliente", b =>
                {
                    b.OwnsOne("NSE.Carrinho.API.Model.Voucher", "Voucher", b1 =>
                        {
                            b1.Property<Guid>("CarrinhoClienteId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Codigo")
                                .HasColumnType("varchar(50)")
                                .HasColumnName("VoucherCodigo");

                            b1.Property<decimal?>("Percentual")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("Percentual");

                            b1.Property<int>("TipoDesconto")
                                .HasColumnType("int")
                                .HasColumnName("TipoDesconto");

                            b1.Property<decimal?>("ValorDesconto")
                                .HasColumnType("decimal(18,2)")
                                .HasColumnName("ValorDesconto");

                            b1.HasKey("CarrinhoClienteId");

                            b1.ToTable("CarrinhoCliente");

                            b1.WithOwner()
                                .HasForeignKey("CarrinhoClienteId");
                        });

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("NSE.Carrinho.API.Models.CarrinhoItem", b =>
                {
                    b.HasOne("NSE.Carrinho.API.Models.CarrinhoCliente", "CarrinhoCliente")
                        .WithMany("Itens")
                        .HasForeignKey("CarrinhoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CarrinhoCliente");
                });

            modelBuilder.Entity("NSE.Carrinho.API.Models.CarrinhoCliente", b =>
                {
                    b.Navigation("Itens");
                });
#pragma warning restore 612, 618
        }
    }
}
