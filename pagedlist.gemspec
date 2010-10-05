version = File.read(File.expand_path("../VERSION",__FILE__)).strip  

Gem::Specification.new do |spec|
  spec.platform    = Gem::Platform::RUBY
  spec.name        = 'PagedList'
  spec.version     = version
  spec.files 	   = Dir['lib/**/*'] + Dir['docs/**/*']  

  spec.summary     = 'Simplified Paged Lists using IEnumerable Collections.'
  spec.description = <<-EOF
	PagedList makes it easier for .Net developers to write paging code. It allows you to take any IEnumerable<T> and by 
	specifying the page size and desired page index, select only a subset of that list. PagedList also provides properties 
	that are useful when building UI paging controls.
  EOF

  spec.authors           = 'Troy Goode'
  spec.email             = 'troygoode@gmail.com'
  spec.homepage          = 'http://github.com/TroyGoode/PagedList'
  spec.rubyforge_project = 'pagedlist'
end  