using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Carrinho.API.Models;

namespace NSE.Carrinho.API.Data.Configurations
{
    public class CarrinhoClienteConfiguration : IEntityTypeConfiguration<CarrinhoCliente>
    {
        public void Configure(EntityTypeBuilder<CarrinhoCliente> builder)
        {
            builder.HasIndex(c => c.ClienteId)
                .HasDatabaseName("IDX_Cliente");

            builder.HasMany(c => c.Itens)
                .WithOne(c => c.CarrinhoCliente)
                .HasForeignKey(c => c.CarrinhoId);

            builder.OwnsOne(c => c.Voucher, v =>
            {
                v.Property(vc => vc.Codigo)
                    .HasColumnName("VoucherCodigo")
                    .HasColumnType("varchar(50)");

                v.Property(vc => vc.TipoDesconto)
                    .HasColumnName("TipoDesconto");

                v.Property(vc => vc.Percentual)
                    .HasColumnName("Percentual");

                v.Property(vc => vc.ValorDesconto)
                    .HasColumnName("ValorDesconto");
            });
        }
    }
}
