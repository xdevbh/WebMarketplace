using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Companies
{
    public class CompanyImage : CreationAuditedEntity
    {
        public virtual Guid CompanyId { get; set; }
        public virtual string BlobName { get; set; }
        public virtual bool IsDefault { get; set; }

        protected CompanyImage()
        {
        }

        public CompanyImage(Guid companyId, string blobName, bool isDefault)
        {
            CompanyId = companyId;
            BlobName = blobName;
            IsDefault = isDefault;
        }

        public override object?[] GetKeys()
        {
            return new object?[] { CompanyId, BlobName };
        }
    }
}
