//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Student_Report_Management.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Year_Master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Year_Master()
        {
            this.tbl_Semister_Master = new HashSet<tbl_Semister_Master>();
        }
    
        public int Year_Id { get; set; }
        public string Year_Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Semister_Master> tbl_Semister_Master { get; set; }
    }
}
